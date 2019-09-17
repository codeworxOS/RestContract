using System;
using System.Linq;
using Codeworx.Rest.Client;
using Codeworx.Rest.Client.Builder;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class CodeworxRestClientServiceCollectionExtensions
    {
        public static IServiceCollection AddOrReplace<TContract>(this IServiceCollection services, ServiceLifetime lifetime, Func<IServiceProvider, TContract> factory)
            where TContract : class
        {
            var found = services.FirstOrDefault(p => p.ServiceType == typeof(TContract) && p.Lifetime == lifetime);

            var service = ServiceDescriptor.Describe(typeof(TContract), factory, lifetime);

            if (found != null)
            {
                var index = services.IndexOf(found);
                services.RemoveAt(index);
                services.Insert(index, service);
            }
            else
            {
                services.Add(service);
            }

            return services;
        }

        public static IRestOptionsBuilder AddRestClient(this IServiceCollection services, string baseUrl = null)
        {
            IRestOptionsBuilder builder = new RestOptionsBuilder(services);
            builder.Services.AddSingleton(typeof(RestOptions<>), typeof(DefaultRestOptions<>));
            builder.Services.AddSingleton<RestOptions>();

            if (!string.IsNullOrWhiteSpace(baseUrl))
            {
                builder = builder.WithBaseUrl(baseUrl);
            }

            return builder;
        }
    }
}