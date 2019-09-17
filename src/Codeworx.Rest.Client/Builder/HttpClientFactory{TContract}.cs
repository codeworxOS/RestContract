using System.Net.Http;

namespace Codeworx.Rest.Client.Builder
{
    public delegate HttpClient HttpClientFactory<TContract>()
        where TContract : class;
}