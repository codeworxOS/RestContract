using System;
using System.Linq;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class CodeworxRestClientInternalServiceCollectionExtensions
    {
        internal static IServiceCollection AddOrReplace<TContract>(this IServiceCollection services, ServiceLifetime lifetime, Func<IServiceProvider, TContract> factory)
                    where TContract : class
        {
            return services.AddOrReplace<TContract, TContract>(lifetime, factory);
        }

        internal static IServiceCollection AddOrReplace<TContract, TImplementation>(this IServiceCollection services, ServiceLifetime lifetime, Func<IServiceProvider, TContract> factory)
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
    }
}
