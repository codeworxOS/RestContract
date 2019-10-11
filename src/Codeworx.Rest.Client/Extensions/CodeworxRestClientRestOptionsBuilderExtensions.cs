using System;
using System.Net.Http;
using System.Reflection;
using Codeworx.Rest;
using Codeworx.Rest.Client.Builder;
using Codeworx.Rest.Client.Formatters;
using Newtonsoft.Json;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class CodeworxRestClientRestOptionsBuilderExtensions
    {
        public static IRestOptionsBuilder AddJsonFormatter(this IRestOptionsBuilder builder, Action<JsonSerializerSettings> options)
        {
            builder.Services.AddOrReplace<IContentFormatter, JsonContentFormatter>(ServiceLifetime.Singleton, sp =>
            {
                var settings = new JsonSerializerSettings();
                options(settings);
                return new JsonContentFormatter(settings);
            });
            return builder;
        }

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

        public static IRestOptionsBuilder DefaultFormatter(this IRestOptionsBuilder builder, string mimeType)
        {
            builder.Services.AddOrReplace<DefaultFormatterSelector>(ServiceLifetime.Scoped, sp => () => mimeType);
            return builder;
        }

        public static IRestOptionsBuilder DefaultFormatter(this IRestOptionsBuilder builder, Func<IServiceProvider, string> mimeTypeSelector)
        {
            builder.Services.AddOrReplace<DefaultFormatterSelector>(ServiceLifetime.Scoped, sp => () => mimeTypeSelector(sp));
            return builder;
        }

        public static IRestOptionsBuilder WithBaseUrl(this IRestOptionsBuilder builder, string baseUrl)
        {
            builder.Services.AddOrReplace<HttpClientFactory>(ServiceLifetime.Scoped, sp => () => new HttpClient { BaseAddress = new Uri(baseUrl) });
            return builder;
        }

        public static IRestOptionsBuilder<TContract> WithBaseUrl<TContract>(this IRestOptionsBuilder<TContract> builder, string baseUrl)
            where TContract : class
        {
            builder.Services.AddOrReplace<HttpClientFactory<TContract>>(ServiceLifetime.Scoped, sp => () => new HttpClient { BaseAddress = new Uri(baseUrl) });
            return builder;
        }

        public static IRestOptionsBuilder<TContract> WithHttpClient<TContract>(this IRestOptionsBuilder<TContract> builder, Func<IServiceProvider, HttpClient> clientFactory)
            where TContract : class
        {
            builder.Services.AddOrReplace<HttpClientFactory<TContract>>(ServiceLifetime.Scoped, sp => () => clientFactory(sp));
            return builder;
        }

        public static IRestOptionsBuilder WithHttpClient(this IRestOptionsBuilder builder, Func<IServiceProvider, HttpClient> clientFactory)
        {
            builder.Services.AddOrReplace<HttpClientFactory>(ServiceLifetime.Scoped, sp => () => clientFactory(sp));
            return builder;
        }
    }
}