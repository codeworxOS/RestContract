using System;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Codeworx.Rest.Client
{
    public class RestClient<TContract>
    {
        private readonly RestOptions _options;

        public RestClient(RestOptions<TContract> options)
        {
            this._options = options;
        }

        public RestClient(RestOptions options)
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

            throw new InvalidOperationException("Unexpected http status code.");
        }

        public async Task<TResult> CallAsync<TResult>(Expression<Func<TContract, Task<TResult>>> operationSelector)
        {
            HttpResponseMessage response = await GetResponse(operationSelector);
            if (response.IsSuccessStatusCode)
            {
                JsonSerializer serializer = GetSerializer();

                using (var stream = await response.Content.ReadAsStreamAsync())
                using (var streamReader = new StreamReader(stream))
                using (var jsonReader = new JsonTextReader(streamReader))
                {
                    var result = serializer.Deserialize<TResult>(jsonReader);

                    return result;
                }
            }

            throw new InvalidOperationException("Unexpected http status code.");
        }

        private static JsonSerializer GetSerializer()
        {
            return JsonSerializer.Create(new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() });
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

            var httpMethod = attribute.HttpMethod();

            HttpContent content = null;

            for (int i = 0; i < methodCall.Arguments.Count; i++)
            {
                var data = Expression.Lambda<Func<object>>(Expression.Convert(methodCall.Arguments[i], typeof(object))).Compile()();

                if (methodCall.Method.GetParameters()[i].GetCustomAttribute<BodyMemberAttribute>() != null)
                {
                    using (var ms = new MemoryStream())
                    {
                        var serializer = GetSerializer();
                        using (var jw = new JsonTextWriter(new StreamWriter(ms)))
                        {
                            serializer.Serialize(jw, data, methodCall.Arguments[i].Type);
                        }

                        content = new ByteArrayContent(ms.ToArray());
                    }

                    content.Headers.ContentType = MediaTypeWithQualityHeaderValue.Parse("application/json");
                }
                else
                {
                    var stringValue = data.ToString();

                    requestUrl = requestUrl.Replace($"{{{methodCall.Method.GetParameters()[i].Name}}}", stringValue);
                }
            }

            var request = new HttpRequestMessage(new HttpMethod(httpMethod), requestUrl);
            request.Headers.Accept.Add(MediaTypeWithQualityHeaderValue.Parse("application/json"));
            if (content != null)
            {
                request.Content = content;
            }

            var response = await client.SendAsync(request);
            return response;
        }
    }
}