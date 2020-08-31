using Codeworx.Rest.AspNetCore;
using Codeworx.Rest.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class CodeworxRestAspNetCoreMvcBuilderExtensions
    {
        public static IMvcCoreBuilder AddRestContract(this IMvcCoreBuilder builder)
        {
            builder = builder.ConfigureApplicationPartManager(p => p.FeatureProviders.Add(new ContractControllerFeatureProvider()));
            builder.Services.AddRestContractServices();

            return builder;
        }

        public static IMvcBuilder AddRestContract(this IMvcBuilder builder)
        {
            builder = builder.ConfigureApplicationPartManager(p => p.FeatureProviders.Add(new ContractControllerFeatureProvider()));
            builder.Services.AddRestContractServices();

            return builder;
        }

        private static IServiceCollection AddRestContractServices(this IServiceCollection services)
        {
            return services.AddSingleton<IApplicationModelProvider, ContractModelProvider>()
                    .AddSingleton<IAttributeMetadataProvider, RestRouteMetadataProvider>()
                    .AddSingleton<IAttributeMetadataProvider, RestOperationMetadataProvider>()
                    .AddSingleton<IAttributeMetadataProvider, BodyMemberMetadataProvider>()
                    .AddSingleton<IAttributeMetadataProvider, AnonymousMetadataProvider>()
                    .AddSingleton<IAttributeMetadataProvider, PolicyMetadataProvider>();
        }
    }
}