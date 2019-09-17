using System;
using System.Net.Http;
using System.Reflection;
using Codeworx.Rest;
using Codeworx.Rest.Client;
using Codeworx.Rest.Client.Builder;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class CodeworxRestClientRestOptionsBuilderExtensions
    {
        public static IRestOptionsBuilder AddRestProxies(this IRestOptionsBuilder options, Assembly proxyAssembly)
        {
            foreach (var item in proxyAssembly.GetCustomAttributes<RestProxyAttribute>())
            {
                options.Services.AddScoped(item.ContractType, item.ProxyType);
            }

            return options;
        }

        public static IRestOptionsBuilder Contract<TContract>(this IRestOptionsBuilder builder, Action<IRestOptionsBuilder<TContract>> subBuilder)
            where TContract : class
        {
            var sub = new RestOptionsBuilder<TContract>(builder.Services);
            subBuilder(sub);

            return builder;
        }

        public static IRestOptionsBuilder WithBaseUrl(this IRestOptionsBuilder builder, string baseUrl)
        {
            builder.Services.AddOrReplace<HttpClientFactory>(ServiceLifetime.Singleton, sp => () => new HttpClient { BaseAddress = new Uri(baseUrl) });
            return builder;
        }

        public static IRestOptionsBuilder<TContract> WithBaseUrl<TContract>(this IRestOptionsBuilder<TContract> builder, string baseUrl)
            where TContract : class
        {
            builder.Services.AddOrReplace<HttpClientFactory<TContract>>(ServiceLifetime.Singleton, sp => () => new HttpClient { BaseAddress = new Uri(baseUrl) });
            return builder;
        }

        public static IRestOptionsBuilder<TContract> WithHttpClient<TContract>(this IRestOptionsBuilder<TContract> builder, Func<IServiceProvider, HttpClient> clientFactory)
            where TContract : class
        {
            builder.Services.AddOrReplace<HttpClientFactory<TContract>>(ServiceLifetime.Singleton, sp => () => clientFactory(sp));
            return builder;
        }

        public static IRestOptionsBuilder WithHttpClient(this IRestOptionsBuilder builder, Func<IServiceProvider, HttpClient> clientFactory)
        {
            builder.Services.AddOrReplace<HttpClientFactory>(ServiceLifetime.Singleton, sp => () => clientFactory(sp));
            return builder;
        }
    }
}