using System;
using Codeworx.Rest;
using Codeworx.Rest.Formatters.Newtonsoft;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class CodeworxRestFormattersNewtonsoftRestOptionsBuilderExtensions
    {
        public static IRestOptionsBuilder AddNewtonsoftJsonFormatter(this IRestOptionsBuilder builder, Action<JsonSerializerSettings> options)
        {
            builder.Services.AddOrReplace<IContentFormatter, NewtonsoftJsonContentFormatter>(ServiceLifetime.Singleton, sp =>
            {
                var settings = new JsonSerializerSettings();
                options(settings);
                return new NewtonsoftJsonContentFormatter(settings);
            });
            return builder;
        }

        public static IRestOptionsBuilder AddNewtonsoftJsonFormatter(this IRestOptionsBuilder builder)
        {
            return builder.AddNewtonsoftJsonFormatter(CreateDefaultJsonSettings);
        }

        private static void CreateDefaultJsonSettings(JsonSerializerSettings settings)
        {
            settings.ContractResolver = new CamelCasePropertyNamesContractResolver();
        }
    }
}
