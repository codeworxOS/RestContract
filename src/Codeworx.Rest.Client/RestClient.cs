using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Codeworx.Rest.Internal;

namespace Codeworx.Rest.Client
{
    public class RestClient<TContract>
        where TContract : class
    {
        private static readonly Regex _parameterRegex;
        private readonly RestOptions _options;

        static RestClient()
        {
            _parameterRegex = new Regex(@"{(?<parameterName>[^:\?}]*)[^}]*}");
        }

        public RestClient(RestOptions<TContract> options)
        {
            this._options = options;
        }

        protected RestClient(RestOptions options)
        {
            this._options = options;
        }

        public RestOptions Options => _options;

        public async Task CallAsync(Expression<Func<TContract, Task>> operationSelector)
        {
            HttpResponseMessage response = await GetResponse(operationSelector);
            if (response.IsSuccessStatusCode)
            {
                return;
            }

            var supportedResponseTypes = GetSupportedResponseTypes(operationSelector);
            if (supportedResponseTypes.TryGetValue((int)response.StatusCode, out var responseType))
            {
                var error = await DeserializeServiceError(response, responseType);
                throw error;
            }

            throw new UnexpectedHttpStatusCodeException(response.StatusCode);
        }

        public async Task<TResult> CallAsync<TResult>(Expression<Func<TContract, Task<TResult>>> operationSelector)
        {
            HttpResponseMessage response = await GetResponse(operationSelector);
            if (response.IsSuccessStatusCode)
            {
                if (response.StatusCode == System.Net.HttpStatusCode.NoContent
                    || response.RequestMessage.Method == HttpMethod.Head)
                {
                    return default(TResult);
                }

                return await GetPayload<TResult>(response);
            }

            var supportedResponseTypes = GetSupportedResponseTypes(operationSelector);
            if (supportedResponseTypes.TryGetValue((int)response.StatusCode, out var responseType))
            {
                var error = await DeserializeServiceError(response, responseType);
                throw error;
            }

            throw new UnexpectedHttpStatusCodeException(response.StatusCode);
        }

        private static bool IsMatch(string templateValue, string parameterName)
        {
            return _parameterRegex.IsMatch(templateValue) && _parameterRegex.Match(templateValue).Groups["parameterName"].Value == parameterName;
        }

        private async Task<Exception> DeserializeServiceError(HttpResponseMessage response, Type responseType)
        {
            Expression<Func<RestClient<TContract>, Task<Exception>>> exp = p => p.DeserializeServiceError<object>(null);
            var methodInfo = ((MethodCallExpression)exp.Body).Method.GetGenericMethodDefinition();

            var toCall = methodInfo.MakeGenericMethod(responseType).Invoke(this, new object[] { response }) as Task<Exception>;

            return await toCall;
        }

        private async Task<Exception> DeserializeServiceError<TResult>(HttpResponseMessage response)
        {
            var payload = await GetPayload<TResult>(response);
            var error = Options.ErrorDispatcher.GetException<TResult>(payload);

            return error;
        }

        private async Task<TResult> GetPayload<TResult>(HttpResponseMessage response)
        {
            if (TypeKey<TResult>.Key == TypeKey<Stream>.Key)
            {
                var resultStream = await response.Content.ReadAsStreamAsync();
                return (TResult)(object)resultStream;
            }

            var formatter = _options.GetFormatter(response.Content?.Headers?.ContentType?.MediaType);

            var result = await formatter.DeserializeAsync<TResult>(response);
            return result;
        }

        private async Task<HttpResponseMessage> GetResponse(LambdaExpression operationSelector)
        {
            var methodCall = operationSelector.Body as MethodCallExpression;

            var param = operationSelector.Parameters.First();

            if (param != methodCall?.Object)
            {
                throw new NotSupportedException();
            }

            var attribute = methodCall.Method.GetCustomAttributes().OfType<RestOperationAttribute>().FirstOrDefault();

            if (attribute == null)
            {
                throw new NotSupportedException();
            }

            var client = _options.GetHttpClient();

            string requestUrl = null;

            if (attribute.Template != null && attribute.Template.StartsWith("/"))
            {
                requestUrl = attribute.Template;
            }
            else
            {
                requestUrl = $"{typeof(TContract).GetCustomAttribute<RestRouteAttribute>().RoutePrefix}/{attribute.Template}";
            }

            var evaluator = new ParameterMatchEvaluator(methodCall, Options.GetAdditionData());

            requestUrl = _parameterRegex.Replace(requestUrl, evaluator.Evaluate);

            var tempUrl = $"http://unknown/{requestUrl.TrimStart('/')}";
            var uri = new Uri(tempUrl, UriKind.Absolute);

            var parsedQuery = from q in uri.Query.TrimStart('?').Split('&')
                              let keyValue = q.Split('=')
                              where keyValue.Length == 2
                              select new { Key = Uri.UnescapeDataString(keyValue[0]), Value = Uri.UnescapeDataString(keyValue[1]) };

            var query = parsedQuery.GroupBy(p => p.Key)
                            .ToDictionary(p => p.Key, p => p.Select(x => x.Value).ToList());

            var httpMethod = attribute.HttpMethod();

            evaluator.AddMissingQueryParameters(query);
            var formatter = _options.GetFormatter();

            requestUrl = $"{uri.AbsolutePath.TrimStart('/')}";
            if (query.Any())
            {
                var data = from q in query
                           from v in q.Value
                           where v != null
                           select $"{Uri.EscapeDataString(q.Key)}={Uri.EscapeDataString(v)}";

                requestUrl += $"?{string.Join("&", data)}";
            }

            var request = new HttpRequestMessage(new HttpMethod(httpMethod), requestUrl);
            request.Headers.Accept.Add(MediaTypeWithQualityHeaderValue.Parse(formatter.MimeType));

            if (evaluator.TryGetBody(out var body, out var type))
            {
                if (body is Stream stream)
                {
                    request.Content = new StreamContent(stream);
                    request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                }
                else
                {
                    await formatter.SerializeAsync(type, body, request);
                }
            }
            else if (request.Method == HttpMethod.Put || request.Method == HttpMethod.Post)
            {
                request.Content = new ByteArrayContent(new byte[0]);
            }

            HttpResponseMessage response = null;

            if (evaluator.CancellationToken.HasValue)
            {
                response = await client.SendAsync(request, evaluator.CancellationToken.Value);
            }
            else
            {
                response = await client.SendAsync(request);
            }

            return response;
        }

        private IDictionary<int, Type> GetSupportedResponseTypes(LambdaExpression operationSelector)
        {
            var methodCall = operationSelector.Body as MethodCallExpression;

            var param = operationSelector.Parameters.First();

            if (param != methodCall?.Object)
            {
                throw new NotSupportedException();
            }

            var result = methodCall.Method.GetCustomAttributes().OfType<ResponseTypeAttribute>()
                                .ToDictionary(p => p.StatusCode, p => p.PayloadType);

            return result;
        }

        private class ParameterMatchEvaluator
        {
            private readonly IReadOnlyDictionary<string, object> _additionalParameters;
            private readonly Dictionary<string, ParameterData> _parameterValues;
            private MethodCallExpression _methodCall;
            private HashSet<string> _usedParameters;

            public ParameterMatchEvaluator(MethodCallExpression methodCall, IReadOnlyDictionary<string, object> additionalParameters)
                : base()
            {
                _methodCall = methodCall;
                _usedParameters = new HashSet<string>();

                _additionalParameters = additionalParameters;
                _parameterValues = methodCall.Method.GetParameters()
                                    .Select((p, i) => new { Parameter = p, Index = i })
                                    .ToDictionary(
                                        p => p.Parameter.Name,
                                        p => new
                                        ParameterData
                                        {
                                            ParameterType = p.Parameter.ParameterType,
                                            Data = Expression.Lambda<Func<object>>(Expression.Convert(methodCall.Arguments[p.Index], typeof(object))).Compile()(),
                                            IsBodyMember = p.Parameter.GetCustomAttribute<BodyMemberAttribute>() != null
                                        });

                var cancellationTokenParameter = _parameterValues.Where(p => p.Value.Data is CancellationToken).ToList();

                if (cancellationTokenParameter.Count > 1)
                {
                    throw new NotSupportedException("Only a maximum of one CancellationToken parameter is supported.");
                }
                else if (cancellationTokenParameter.Count == 1)
                {
                    _parameterValues.Remove(cancellationTokenParameter[0].Key);
                    CancellationToken = (CancellationToken)cancellationTokenParameter[0].Value.Data;
                }
            }

            public CancellationToken? CancellationToken { get; }

            public void AddMissingQueryParameters(IDictionary<string, List<string>> queryParameters)
            {
                var missing = _parameterValues
                    .Where(p => !p.Value.IsBodyMember && !_usedParameters.Contains(p.Key))
                    .ToList();

                foreach (var param in missing)
                {
                    queryParameters.Add(param.Key, new List<string> { GetDataStringValue(param.Value.Data) });
                }
            }

            public string Evaluate(Match match)
            {
                var parameterName = match.Groups["parameterName"].Value;
                if (_parameterValues.TryGetValue(parameterName, out var value))
                {
                    if (value.IsBodyMember)
                    {
                        throw new TemplateParseException($"Parameter {parameterName} was found in the template but is marked as BodyMember.");
                    }

                    _usedParameters.Add(parameterName);
                    return HttpUtility.UrlPathEncode(GetDataStringValue(value.Data));
                }
                else if (_additionalParameters.TryGetValue(parameterName, out var additionalValue))
                {
                    return HttpUtility.UrlPathEncode(GetDataStringValue(additionalValue));
                }

                throw new TemplateParseException($"Parameter {parameterName} not found on method {_methodCall.Method}.");
            }

            public bool TryGetBody(out object value, out Type valueType)
            {
                var bodyMembers = _parameterValues.Where(p => p.Value.IsBodyMember)
                                    .ToDictionary(p => p.Key, p => p.Value);

                if (bodyMembers.Count == 1)
                {
                    value = bodyMembers.Values.First().Data;
                    valueType = bodyMembers.Values.First().ParameterType;
                    return true;
                }
                else if (bodyMembers.Count > 1)
                {
                    value = bodyMembers.ToDictionary(p => p.Key, p => p.Value.Data);
                    valueType = typeof(Dictionary<string, object>);
                    return true;
                }

                value = null;
                valueType = null;
                return false;
            }

            private string GetDataStringValue(object data)
            {
                if (data == null)
                {
                    return null;
                }

                var culture = CultureInfo.InvariantCulture;

                switch (data)
                {
                    case string value:
                        return value;

                    case DateTime value:
                        return value.ToString("s", culture);

                    case decimal value:
                        return value.ToString(culture);

                    case double value:
                        return value.ToString(culture);

                    case float value:
                        return value.ToString(culture);
                }

                return data.ToString();
            }

            private class ParameterData
            {
                public object Data { get; set; }

                public bool IsBodyMember { get; set; }

                public Type ParameterType { get; set; }
            }
        }
    }
}