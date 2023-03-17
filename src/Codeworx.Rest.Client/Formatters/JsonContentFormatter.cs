using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;

namespace Codeworx.Rest.Client.Formatters
{
    public class JsonContentFormatter : IContentFormatter
    {
        private readonly JsonSerializerOptions _options;

        public JsonContentFormatter(JsonSerializerOptions options)
        {
            _options = options;
        }

        public string MimeType { get; } = "application/json";

        public async Task<object> DeserializeAsync(Type contentType, HttpResponseMessage response)
        {
            using (var responseStream = await response.Content.ReadAsStreamAsync())
            {
                var result = await JsonSerializer.DeserializeAsync(responseStream, contentType, _options);

                return result;
            }
        }

        public async Task SerializeAsync(Type contentType, object value, HttpRequestMessage request)
        {
            using (var buffer = new MemoryStream())
            {
                await JsonSerializer.SerializeAsync(buffer, value, contentType, _options);

                request.Content = new ByteArrayContent(buffer.ToArray());
                request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse(MimeType);
            }
        }
    }
}