using Codeworx.Rest.Client.Builder;

namespace Codeworx.Rest.Client
{
    public class RestOptions<TContract> : RestOptions
        where TContract : class
    {
        public RestOptions(HttpClientFactory<TContract> clientFactory)
            : base(() => clientFactory())
        {
        }
    }
}