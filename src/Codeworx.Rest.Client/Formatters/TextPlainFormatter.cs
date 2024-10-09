using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Codeworx.Rest.Client.Formatters
{
    public class TextPlainFormatter : IContentFormatter
    {
        public TextPlainFormatter()
        {
        }

        public string MimeType { get; } = "text/plain";

        public async Task<object> DeserializeAsync(Type contentType, HttpResponseMessage response)
        {
            if (contentType == typeof(string))
            {
                return await response.Content.ReadAsStringAsync();
            }
            else if (contentType == typeof(Stream))
            {
                return await response.Content.ReadAsStreamAsync();
            }

            throw new NotSupportedException("The text/plain formatter only support string and stream values.");
        }

        public Task SerializeAsync(Type contentType, object value, HttpRequestMessage request)
        {
            if (value is string stringValue)
            {
                request.Content = new StringContent(stringValue);
                request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse(MimeType);
                return Task.CompletedTask;
            }
            else if (value is Stream streamValue)
            {
                request.Content = new StreamContent(streamValue);
                request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse(MimeType);
                return Task.CompletedTask;
            }

            throw new NotSupportedException("The text/plain formatter only support string and stream values.");
        }
    }
}