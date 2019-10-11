using Microsoft.Extensions.DependencyInjection;

namespace Codeworx.Rest.Client.Builder
{
    public class RestOptionsBuilder : IRestOptionsBuilder
    {
        public RestOptionsBuilder(IServiceCollection services)
        {
            Services = services;
        }

        public IServiceCollection Services { get; }
    }
}