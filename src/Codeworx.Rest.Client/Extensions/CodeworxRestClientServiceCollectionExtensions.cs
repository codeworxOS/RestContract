using System.Linq;
using Codeworx.Rest;
using Codeworx.Rest.Client;
using Codeworx.Rest.Client.Builder;
using Codeworx.Rest.Client.Formatters;
using Codeworx.Rest.Internal;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class CodeworxRestClientServiceCollectionExtensions
    {
        public static IRestOptionsBuilder AddRestClient(this IServiceCollection services, string baseUrl = null)
        {
            IRestOptionsBuilder builder = new RestOptionsBuilder(services);
            builder.Services.AddScoped(typeof(HttpClientGroupService));
            builder.Services.AddScoped(typeof(RestOptions<>), typeof(DefaultRestOptions<>));
            builder.Services.AddScoped(typeof(RestClient<>));
            builder.Services.AddScoped<RestOptions>();
            builder.Services.AddSingleton<IContentFormatter, JsonContentFormatter>(sp => new JsonContentFormatter(new System.Text.Json.JsonSerializerOptions(System.Text.Json.JsonSerializerDefaults.Web)));
            builder.Services.AddScoped<DefaultFormatterSelector>(sp => () => "application/json");

            if (!builder.Services.Any(p => p.ServiceType == typeof(IServiceErrorDispatcher)))
            {
                builder.Services.AddSingleton<IServiceErrorDispatcher, ServiceExceptionErrorDispatcher>();
            }

            if (!string.IsNullOrWhiteSpace(baseUrl))
            {
                builder = builder.WithBaseUrl(baseUrl);
            }

            return builder;
        }
    }
}
