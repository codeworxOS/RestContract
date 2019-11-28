using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

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

        public async Task CallAsync(Expression<Func<TContract, Task>> operationSelector)
        {
            HttpResponseMessage response = await GetResponse(operationSelector);
            if (response.IsSuccessStatusCode)
            {
                return;
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

                var formatter = _options.GetFormatter(response.Content?.Headers?.ContentType?.MediaType);

                var result = await formatter.DeserializeAsync<TResult>(response);
                return result;
            }

            throw new UnexpectedHttpStatusCodeException(response.StatusCode);
        }

        private static bool IsMatch(string templateValue, string parameterName)
        {
            return _parameterRegex.IsMatch(templateValue) && _parameterRegex.Match(templateValue).Groups["parameterName"].Value == parameterName;
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

            var evaluator = new ParameterMatchEvaluator(methodCall);

            requestUrl = _parameterRegex.Replace(requestUrl, evaluator.Evaluate);

            var tempUrl = $"http://unknown/{requestUrl.TrimStart('/')}";
            var uri = new Uri(tempUrl, UriKind.Absolute);

            var query = HttpUtility.ParseQueryString(uri.Query);
            var httpMethod = attribute.HttpMethod();

            evaluator.AddMissingQueryParameters(query);
            var formatter = _options.GetFormatter();

            var builder = new UriBuilder();
            builder.Path = uri.AbsolutePath;
            builder.Query = query.ToString();
            requestUrl = builder.Uri.PathAndQuery.TrimStart('/');

            var request = new HttpRequestMessage(new HttpMethod(httpMethod), requestUrl);
            request.Headers.Accept.Add(MediaTypeWithQualityHeaderValue.Parse(formatter.MimeType));

            if (evaluator.TryGetBody(out var body, out var type))
            {
                await formatter.SerializeAsync(type, body, request);
            }
            else if (request.Method == HttpMethod.Put || request.Method == HttpMethod.Post)
            {
                request.Content = new ByteArrayContent(new byte[0]);
            }

            var response = await client.SendAsync(request);
            return response;
        }

        private class ParameterMatchEvaluator
        {
            private readonly Dictionary<string, ParameterData> _parameterValues;
            private MethodCallExpression _methodCall;
            private HashSet<string> _usedParameters;

            public ParameterMatchEvaluator(MethodCallExpression methodCall)
                : base()
            {
                _methodCall = methodCall;
                _usedParameters = new HashSet<string>();

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
            }

            public void AddMissingQueryParameters(NameValueCollection queryParameters)
            {
                var missing = _parameterValues
                    .Where(p => !p.Value.IsBodyMember && !_usedParameters.Contains(p.Key))
                    .ToList();

                foreach (var param in missing)
                {
                    queryParameters[param.Key] = GetDataStringValue(param.Value.Data);
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
                    return HttpUtility.UrlEncode(GetDataStringValue(value.Data));
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