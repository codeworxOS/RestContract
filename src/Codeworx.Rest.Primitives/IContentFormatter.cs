using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Codeworx.Rest
{
    public interface IContentFormatter
    {
        string MimeType { get; }

        Task<object> DeserializeAsync(Type contentType, HttpResponseMessage response);

        Task SerializeAsync(Type contentType, object value, HttpRequestMessage request);
    }
}