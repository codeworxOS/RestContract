using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Codeworx.Rest.Client.Formatters
{
    public class JsonContentFormatter : IContentFormatter
    {
        private readonly JsonSerializerSettings _settings;

        public JsonContentFormatter(JsonSerializerSettings settings)
        {
            _settings = settings;
        }

        public string MimeType { get; } = "application/json";

        public async Task<object> DeserializeAsync(Type contentType, HttpResponseMessage response)
        {
            var serializer = GetSerializer();

            using (var responseStream = await response.Content.ReadAsStreamAsync())
            using (var streamReader = new StreamReader(responseStream))
            using (var jsonReader = new JsonTextReader(streamReader))
            {
                return serializer.Deserialize(jsonReader, contentType);
            }
        }

        public Task SerializeAsync(Type contentType, object value, HttpRequestMessage request)
        {
            var serializer = GetSerializer();

            using (var buffer = new MemoryStream())
            {
                using (var streamWriter = new StreamWriter(buffer))
                using (var jsonWriter = new JsonTextWriter(streamWriter))
                {
                    serializer.Serialize(jsonWriter, value, contentType);
                }

                request.Content = new ByteArrayContent(buffer.ToArray());
                request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse(MimeType);
            }

            return Task.CompletedTask;
        }

        private JsonSerializer GetSerializer()
        {
            return JsonSerializer.Create(_settings);
        }
    }
}