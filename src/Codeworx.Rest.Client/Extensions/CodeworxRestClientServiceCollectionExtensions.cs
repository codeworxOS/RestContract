using System;
using System.Linq;
using Codeworx.Rest;
using Codeworx.Rest.Client;
using Codeworx.Rest.Client.Builder;
using Codeworx.Rest.Client.Formatters;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class CodeworxRestClientServiceCollectionExtensions
    {
        public static IServiceCollection AddOrReplace<TContract>(this IServiceCollection services, ServiceLifetime lifetime, Func<IServiceProvider, TContract> factory)
            where TContract : class
        {
            return services.AddOrReplace<TContract, TContract>(lifetime, factory);
        }

        public static IServiceCollection AddOrReplace<TContract, TImplementation>(this IServiceCollection services, ServiceLifetime lifetime, Func<IServiceProvider, TContract> factory)
            where TContract : class
            where TImplementation : TContract
        {
            var found = services.FirstOrDefault(p => p.ServiceType == typeof(TContract) && (p.ImplementationType ?? p.ImplementationFactory?.Method?.ReturnType) == typeof(TImplementation) && p.Lifetime == lifetime);

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
            builder.Services.AddScoped(typeof(RestOptions<>), typeof(DefaultRestOptions<>));
            builder.Services.AddScoped(typeof(RestClient<>));
            builder.Services.AddScoped<RestOptions>();
            builder.Services.AddSingleton<IContentFormatter, JsonContentFormatter>(sp => new JsonContentFormatter(CreateDefaultJsonSettings()));
            builder.Services.AddScoped<DefaultFormatterSelector>(sp => () => "application/json");

            if (!string.IsNullOrWhiteSpace(baseUrl))
            {
                builder = builder.WithBaseUrl(baseUrl);
            }

            return builder;
        }

        private static JsonSerializerSettings CreateDefaultJsonSettings()
        {
            var settings = new JsonSerializerSettings();
            settings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            return settings;
        }
    }
}
