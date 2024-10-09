using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text.Json;
using Codeworx.Rest;
using Codeworx.Rest.Client.Builder;
using Codeworx.Rest.Client.Formatters;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class CodeworxRestClientRestOptionsBuilderExtensions
    {
        public static IRestOptionsBuilder AddTextPlainFormatter(this IRestOptionsBuilder builder)
        {
            builder.Services.AddOrReplace<IContentFormatter, TextPlainFormatter>(ServiceLifetime.Singleton, sp => new TextPlainFormatter());
            return builder;
        }

        public static IRestOptionsBuilder AddJsonFormatter(this IRestOptionsBuilder builder)
        {
            return builder.AddJsonFormatter(_ => { });
        }

        public static IRestOptionsBuilder AddJsonFormatter(this IRestOptionsBuilder builder, Action<JsonSerializerOptions> options)
        {
            builder.Services.AddOrReplace<IContentFormatter, JsonContentFormatter>(ServiceLifetime.Singleton, sp =>
            {
                var settings = new JsonSerializerOptions(JsonSerializerDefaults.Web);
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

        public static IRestOptionsBuilder<TContract> DefaultFormatter<TContract>(this IRestOptionsBuilder<TContract> builder, string mimeType)
            where TContract : class
        {
            builder.Services.AddOrReplace<DefaultFormatterSelector<TContract>>(ServiceLifetime.Scoped, sp => () => mimeType);
            return builder;
        }

        public static IRestOptionsBuilder<TContract> DefaultFormatter<TContract>(this IRestOptionsBuilder<TContract> builder, Func<IServiceProvider, string> mimeTypeSelector)
            where TContract : class
        {
            builder.Services.AddOrReplace<DefaultFormatterSelector<TContract>>(ServiceLifetime.Scoped, sp => () => mimeTypeSelector(sp));
            return builder;
        }

        public static IRestOptionsBuilder Group(this IRestOptionsBuilder builder, string groupId, Action<IGroupRestOptionsBuilder> subBuilder)
        {
            var sub = new GroupRestOptionsBuilder(builder.Services, groupId);
            subBuilder(sub);

            return builder;
        }

        public static IRestOptionsBuilder WithAdditionalData(this IRestOptionsBuilder builder, IDictionary<string, object> data)
        {
            builder.Services.AddSingleton<IAdditionalDataProvider>(new AdditionalConstantDataProvider(data));
            return builder;
        }

        public static IRestOptionsBuilder WithAdditionalData(this IRestOptionsBuilder builder, Func<IServiceProvider, IDictionary<string, object>> data)
        {
            builder.Services.AddScoped<IAdditionalDataProvider>(sp => new AdditionalDataProviderFactory(sp, data));
            return builder;
        }

        public static IRestOptionsBuilder WithBaseUrl(this IRestOptionsBuilder builder, string baseUrl)
        {
            return builder.WithHttpClient((sp, client) => client.BaseAddress = new Uri(baseUrl), null);
        }

        public static IRestOptionsBuilder WithBaseUrl(this IRestOptionsBuilder builder, string baseUrl, Action<IHttpClientBuilder> httpClientBuilder)
        {
            return builder.WithHttpClient((sp, client) => client.BaseAddress = new Uri(baseUrl), httpClientBuilder);
        }

        public static IRestOptionsBuilder<TContract> WithBaseUrl<TContract>(this IRestOptionsBuilder<TContract> builder, string baseUrl)
    where TContract : class
        {
            return builder.WithHttpClient((sp, client) => client.BaseAddress = new Uri(baseUrl), null);
        }

        public static IRestOptionsBuilder<TContract> WithBaseUrl<TContract>(this IRestOptionsBuilder<TContract> builder, string baseUrl, Action<IHttpClientBuilder> httpClientBuilder)
            where TContract : class
        {
            return builder.WithHttpClient((sp, client) => client.BaseAddress = new Uri(baseUrl), httpClientBuilder);
        }

        public static IGroupRestOptionsBuilder WithBaseUrl(this IGroupRestOptionsBuilder builder, string baseUrl)
        {
            return builder.WithHttpClient((sp, client) => client.BaseAddress = new Uri(baseUrl), null);
        }

        public static IGroupRestOptionsBuilder WithBaseUrl(this IGroupRestOptionsBuilder builder, string baseUrl, Action<IHttpClientBuilder> httpClientBuilder)
        {
            return builder.WithHttpClient((sp, client) => client.BaseAddress = new Uri(baseUrl), httpClientBuilder);
        }

        public static IRestOptionsBuilder<TContract> WithHttpClient<TContract>(this IRestOptionsBuilder<TContract> builder, Action<IServiceProvider, HttpClient> clientFactory)
where TContract : class
        {
            return builder.WithHttpClient(clientFactory, null);
        }

        public static IRestOptionsBuilder<TContract> WithHttpClient<TContract>(this IRestOptionsBuilder<TContract> builder, Action<IServiceProvider, HttpClient> clientFactory, Action<IHttpClientBuilder> httpClientBuilder)
    where TContract : class
        {
            var clientKey = $"restclient.{typeof(TContract).FullName}";
            var clientBuilder = builder.Services.AddHttpClient(clientKey);
            httpClientBuilder?.Invoke(clientBuilder);

            builder.Services.AddOrReplace<HttpClientFactory<TContract>>(ServiceLifetime.Transient, sp => () =>
            {
                var client = sp.GetRequiredService<IHttpClientFactory>().CreateClient(clientKey);
                clientFactory(sp, client);
                return client;
            });

            return builder;
        }

        public static IGroupRestOptionsBuilder WithHttpClient(this IGroupRestOptionsBuilder builder, Action<IServiceProvider, HttpClient> clientFactory)
        {
            return builder.WithHttpClient(clientFactory, null);
        }

        public static IGroupRestOptionsBuilder WithHttpClient(this IGroupRestOptionsBuilder builder, Action<IServiceProvider, HttpClient> clientFactory, Action<IHttpClientBuilder> httpClientBuilder)
        {
            var clientKey = $"restclient.{builder.GroupId}";
            var clientBuilder = builder.Services.AddHttpClient(clientKey);
            httpClientBuilder?.Invoke(clientBuilder);

            var existing = builder.Services.FirstOrDefault(p => p.ServiceType == typeof(HttpClientGroup) && p.ImplementationInstance != null && ((HttpClientGroup)p.ImplementationInstance).GroupId == builder.GroupId);

            if (existing != null)
            {
                builder.Services.Remove(existing);
            }

            builder.Services.AddSingleton(
                new HttpClientGroup(
                builder.GroupId,
                sp =>
                {
                    var client = sp.GetRequiredService<IHttpClientFactory>().CreateClient(clientKey);
                    clientFactory(sp, client);
                    return client;
                }));

            return builder;
        }

        public static IRestOptionsBuilder WithHttpClient(this IRestOptionsBuilder builder, Action<IServiceProvider, HttpClient> clientFactory)
        {
            return builder.WithHttpClient(clientFactory, null);
        }

        public static IRestOptionsBuilder WithHttpClient(this IRestOptionsBuilder builder, Action<IServiceProvider, HttpClient> clientFactory, Action<IHttpClientBuilder> httpClientBuilder)
        {
            var clientBuilder = builder.Services.AddHttpClient("restclient.default");
            httpClientBuilder?.Invoke(clientBuilder);

            builder.Services.AddOrReplace<HttpClientFactory>(ServiceLifetime.Transient, sp => () =>
            {
                var client = sp.GetRequiredService<IHttpClientFactory>().CreateClient("restclient.default");
                clientFactory(sp, client);
                return client;
            });

            return builder;
        }

        public static IRestOptionsBuilder<TContract> WithTestingHttpClient<TContract>(this IRestOptionsBuilder<TContract> builder, Func<IServiceProvider, HttpClient> clientFactory)
            where TContract : class
        {
            builder.Services.AddOrReplace<HttpClientFactory<TContract>>(ServiceLifetime.Scoped, sp => () => clientFactory(sp));
            return builder;
        }

        public static IRestOptionsBuilder WithTestingHttpClient(this IRestOptionsBuilder builder, Func<IServiceProvider, HttpClient> clientFactory)
        {
            builder.Services.AddOrReplace<HttpClientFactory>(ServiceLifetime.Scoped, sp => () => clientFactory(sp));
            return builder;
        }

        private class AdditionalConstantDataProvider : IAdditionalDataProvider
        {
            private IDictionary<string, object> _data;

            public AdditionalConstantDataProvider(IDictionary<string, object> data)
            {
                this._data = data;
            }

            public IDictionary<string, object> GetValues()
            {
                return _data;
            }
        }

        private class AdditionalDataProviderFactory : IAdditionalDataProvider
        {
            private readonly IServiceProvider _serviceProvider;
            private Func<IServiceProvider, IDictionary<string, object>> _factory;

            public AdditionalDataProviderFactory(IServiceProvider serviceProvider, Func<IServiceProvider, IDictionary<string, object>> factory)
            {
                _factory = factory;
                _serviceProvider = serviceProvider;
            }

            public IDictionary<string, object> GetValues()
            {
                return _factory(_serviceProvider);
            }
        }
    }
}