using System.Net.Http;
using Codeworx.Rest.Client;

namespace Codeworx.Rest.UnitTests.TestServerUtilities
{
    public class TestServerRestOptions : RestOptions
    {
        private readonly HttpClient _httpClient;

        public TestServerRestOptions(HttpClient httpClient) 
            : base(string.Empty)
        {
            _httpClient = httpClient;
        }

        public override HttpClient GetHttpClient()
        {
            return _httpClient;
        }
    }
}
