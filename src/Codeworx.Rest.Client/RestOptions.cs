using System.Net.Http;
using Codeworx.Rest.Client.Builder;

namespace Codeworx.Rest.Client
{
    public class RestOptions
    {
        private readonly HttpClientFactory _clientFactory;

        public RestOptions(HttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public virtual HttpClient GetHttpClient()
        {
            return _clientFactory();
        }
    }
}