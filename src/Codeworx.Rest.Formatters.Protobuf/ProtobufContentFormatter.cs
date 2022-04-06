using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using ProtoBuf.Meta;

namespace Codeworx.Rest.Formatters.Protobuf
{
    public class ProtobufContentFormatter : IContentFormatter
    {
        private readonly TypeModel _model;

        public ProtobufContentFormatter(TypeModel model)
        {
            _model = model;
        }

        public string MimeType { get; } = Constants.ProtobufMimeType;

        public async Task<object> DeserializeAsync(Type contentType, HttpResponseMessage response)
        {
            using (var responseStream = await response.Content.ReadAsStreamAsync())
            {
                return _model.Deserialize(responseStream, null, contentType);
            }
        }

        public Task SerializeAsync(Type contentType, object value, HttpRequestMessage request)
        {
            if (value == null)
            {
                request.Headers.Add(Constants.ProtobufNullHeader, "true");
                request.Content = new ByteArrayContent(new byte[] { });
                request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse(MimeType);
                return Task.CompletedTask;
            }

            using (var buffer = new MemoryStream())
            {
                _model.Serialize(buffer, value);
                request.Content = new ByteArrayContent(buffer.ToArray());
                request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse(MimeType);
            }

            return Task.CompletedTask;
        }
    }
}