using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
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
            HttpResponseMessage response = await GetResponse(operationSelector, (TypeKey<TResult>.Key == TypeKey<Stream>.Key) ? HttpCompletionOption.ResponseHeadersRead : HttpCompletionOption.ResponseContentRead);
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

        private async Task<HttpResponseMessage> GetResponse(LambdaExpression operationSelector, HttpCompletionOption completionOptions = HttpCompletionOption.ResponseContentRead)
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

            if (evaluator.TryGetBody(out var body, out var type, out var contentTypes))
            {
                if (body is Stream stream)
                {
                    request.Content = new StreamContent(stream);
                    if (contentTypes.Any())
                    {
                        request.Content.Headers.ContentType = new MediaTypeHeaderValue(contentTypes[0]);
                    }
                    else
                    {
                        request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                    }
                }
                else
                {
                    if (contentTypes.Any())
                    {
                        if (!contentTypes.Contains(formatter.MimeType, StringComparer.OrdinalIgnoreCase))
                        {
                            foreach (var item in contentTypes)
                            {
                                formatter = _options.GetFormatter(item);

                                if (formatter.MimeType.Equals(item, StringComparison.OrdinalIgnoreCase))
                                {
                                    break;
                                }
                                else
                                {
                                    formatter = null;
                                }
                            }
                        }
                    }

                    if (formatter == null)
                    {
                        throw new NotSupportedException($"No matching formatter found.");
                    }

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
                response = await client.SendAsync(request, completionOptions, evaluator.CancellationToken.Value);
            }
            else
            {
                response = await client.SendAsync(request, completionOptions);
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
                                            Key = p.Parameter.Name,
                                            ParameterType = p.Parameter.ParameterType,
                                            Data = Expression.Lambda<Func<object>>(Expression.Convert(methodCall.Arguments[p.Index], typeof(object))).Compile()(),
                                            IsBodyMember = p.Parameter.GetCustomAttribute<BodyMemberAttribute>() != null,
                                            ContentTypes = p.Parameter.GetCustomAttribute<BodyMemberAttribute>()?.ContentTypes ?? new string[] { },
                                            IsQueryMember = p.Parameter.GetCustomAttribute<QueryMemberAttribute>() != null,
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
                    AppendDataStringValues(queryParameters, param.Value);
                }
            }

            public string Evaluate(Match match)
            {
                var parameterName = match.Groups["parameterName"].Value;
                ParameterData parameter = null;

                if (_parameterValues.TryGetValue(parameterName, out var value))
                {
                    if (value.IsBodyMember)
                    {
                        throw new TemplateParseException($"Parameter {parameterName} was found in the template but is marked as BodyMember.");
                    }

                    if (value.IsQueryMember)
                    {
                        throw new TemplateParseException($"Parameter {parameterName} was found in the template but is marked as QueryMember.");
                    }

                    _usedParameters.Add(parameterName);
                    if (value.Data == null)
                    {
                        return null;
                    }

                    parameter = value;
                }
                else if (_additionalParameters.TryGetValue(parameterName, out var additionalValue))
                {
                    parameter = new ParameterData
                    {
                        Key = parameterName,
                        Data = additionalValue,
                        ParameterType = additionalValue?.GetType() ?? typeof(object),
                    };
                }

                if (parameter != null)
                {
                    var values = new Dictionary<string, List<string>>();
                    AppendDataStringValues(values, parameter);
                    if (values.TryGetValue(parameter.Key, out var data))
                    {
                        return Uri.EscapeDataString(string.Join(",", data));
                    }

                    return null;
                }

                throw new TemplateParseException($"Parameter {parameterName} not found on method {_methodCall.Method}.");
            }

            public bool TryGetBody(out object value, out Type valueType)
            {
                return TryGetBody(out value, out valueType, out var contentTypes);
            }

            public bool TryGetBody(out object value, out Type valueType, out string[] contentTypes)
            {
                var bodyMembers = _parameterValues.Where(p => p.Value.IsBodyMember)
                                    .ToDictionary(p => p.Key, p => p.Value);

                if (bodyMembers.Count == 1)
                {
                    value = bodyMembers.Values.First().Data;
                    valueType = bodyMembers.Values.First().ParameterType;
                    contentTypes = bodyMembers.Values.First().ContentTypes;
                    return true;
                }
                else if (bodyMembers.Count > 1)
                {
                    value = bodyMembers.ToDictionary(p => p.Key, p => p.Value.Data);
                    valueType = typeof(Dictionary<string, object>);
                    contentTypes = bodyMembers.Values.First().ContentTypes;
                    return true;
                }

                value = null;
                valueType = null;
                contentTypes = new string[] { };
                return false;
            }

            private void AppendDataStringValues(IDictionary<string, List<string>> values, ParameterData parameter)
            {
                if (values.ContainsKey(parameter.Key))
                {
                    throw new InvalidOperationException($"Key {parameter.Key} already exists in parameters list.");
                }

                if (parameter.Data == null)
                {
                    return;
                }

                var culture = CultureInfo.InvariantCulture;

                if (parameter.Data is string value)
                {
                    values.Add(parameter.Key, new List<string> { value });
                    return;
                }
                else if (parameter.Data is DateTime dateTimeValue)
                {
                    values.Add(parameter.Key, new List<string> { dateTimeValue.ToString("o", culture) });
                    return;
                }
                else if (parameter.Data is DateTimeOffset dateTimeOffsetValue)
                {
                    values.Add(parameter.Key, new List<string> { dateTimeOffsetValue.ToString("o", culture) });
                    return;
                }
                else if (parameter.Data is IEnumerable enumerable)
                {
                    var result = new List<string>();
                    var param = new ParameterData
                    {
                        Key = parameter.Key,
                        ParameterType = parameter.ParameterType.GetEnumerableElementType(),
                        ContentTypes = parameter.ContentTypes,
                    };

                    foreach (var item in enumerable.Cast<object>())
                    {
                        param.Data = item;
                        var temp = new Dictionary<string, List<string>>();
                        AppendDataStringValues(temp, param);
                        if (temp.TryGetValue(param.Key, out var data))
                        {
                            result.AddRange(data);
                        }
                    }

                    values.Add(parameter.Key, result);
                    return;
                }

                var type = Nullable.GetUnderlyingType(parameter.ParameterType) ?? parameter.ParameterType;

                if (type.HasCustomStringConverter(out var converter))
                {
                    var convertedValue = (string)converter.ConvertToInvariantString(parameter.Data);
                    values.Add(parameter.Key, new List<string> { convertedValue });
                }
                else if (parameter.IsQueryMember)
                {
                    var queryKeyValuePairs = parameter.Data.GetType()
                             .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                             .ToDictionary(prop => prop, prop => prop.GetValue(parameter.Data));

                    if (queryKeyValuePairs.Count > 0)
                    {
                        foreach (var item in queryKeyValuePairs.Where(p => p.Value != null))
                        {
                            AppendDataStringValues(values, new ParameterData { Key = item.Key.Name, ParameterType = item.Key.PropertyType, Data = item.Value, ContentTypes = parameter.ContentTypes });
                        }
                    }
                    else
                    {
                        values.Add(parameter.Key, new List<string> { parameter.Data.ToString() });
                    }
                }
                else
                {
                    values.Add(parameter.Key, new List<string> { parameter.Data.ToString() });
                }
            }

            private class ParameterData
            {
                public string[] ContentTypes { get; set; }

                public object Data { get; set; }

                public bool IsBodyMember { get; set; }

                public bool IsQueryMember { get; set; }

                public string Key { get; set; }

                public Type ParameterType { get; set; }
            }
        }
    }
}