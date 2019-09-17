using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Codeworx.Rest.Client.Builder
{
    public class RestOptionsBuilder<TContract> : IRestOptionsBuilder<TContract>
        where TContract : class
    {
        public RestOptionsBuilder(IServiceCollection services)
        {
            services.AddSingleton<RestOptions<TContract>>();
            services.AddOrReplace<HttpClientFactory<TContract>>(ServiceLifetime.Singleton, sp => () => new HttpClient());

            Services = services;
        }

        public IServiceCollection Services { get; }
    }
}