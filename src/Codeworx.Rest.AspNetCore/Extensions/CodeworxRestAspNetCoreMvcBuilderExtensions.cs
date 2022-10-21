using System.Linq;
using Codeworx.Rest;
using Codeworx.Rest.AspNetCore;
using Codeworx.Rest.AspNetCore.Authorization;
using Codeworx.Rest.AspNetCore.Filters;
using Codeworx.Rest.Internal;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class CodeworxRestAspNetCoreMvcBuilderExtensions
    {
        public static IMvcCoreBuilder AddRestContract(this IMvcCoreBuilder builder)
        {
            builder.AddMvcOptions(p => p.Filters.Add(new ErrorResponseFilter()));

            builder = builder.ConfigureApplicationPartManager(p => p.FeatureProviders.Add(new ContractControllerFeatureProvider()));
            builder.Services.AddRestContractServices();

            return builder;
        }

        public static IMvcBuilder AddRestContract(this IMvcBuilder builder)
        {
            builder.AddMvcOptions(p => p.Filters.Add(new ErrorResponseFilter()));
            builder = builder.ConfigureApplicationPartManager(p => p.FeatureProviders.Add(new ContractControllerFeatureProvider()));
            builder.Services.AddRestContractServices();

            return builder;
        }

        private static IServiceCollection AddRestContractServices(this IServiceCollection services)
        {
            services = services.AddSingleton<IApplicationModelProvider, ContractModelProvider>()
                    .AddSingleton<IAttributeMetadataProvider, RestRouteMetadataProvider>()
                    .AddSingleton<IAttributeMetadataProvider, RestOperationMetadataProvider>()
                    .AddSingleton<IAttributeMetadataProvider, BodyMemberMetadataProvider>()
                    .AddSingleton<IAttributeMetadataProvider, AnonymousMetadataProvider>()
                    .AddSingleton<IAttributeMetadataProvider, PolicyMetadataProvider>()
                    .AddSingleton<IAttributeMetadataProvider, ResponseTypeMetadataProvider>()
                    .AddSingleton<IAttributeMetadataProvider, BodyMemberTypeMetadataProvider>();

            if (!services.Any(p => p.ServiceType == typeof(IServiceErrorDispatcher)))
            {
                services = services.AddSingleton<IServiceErrorDispatcher, ServiceExceptionErrorDispatcher>();
            }

            return services;
        }
    }
}