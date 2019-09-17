using Codeworx.Rest.AspNetCore;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class CodeworxRestAspNetCoreMvcBuilderExtensions
    {
        public static IMvcCoreBuilder AddRestContract(this IMvcCoreBuilder builder)
        {
            builder = builder.ConfigureApplicationPartManager(p => p.FeatureProviders.Add(new ContractControllerFeatureProvider()));
            builder.Services.AddSingleton<IApplicationModelProvider, ContractModelProvider>();

            return builder;
        }

        public static IMvcBuilder AddRestContract(this IMvcBuilder builder)
        {
            builder = builder.ConfigureApplicationPartManager(p => p.FeatureProviders.Add(new ContractControllerFeatureProvider()));
            builder.Services.AddSingleton<IApplicationModelProvider, ContractModelProvider>();

            return builder;
        }
    }
}