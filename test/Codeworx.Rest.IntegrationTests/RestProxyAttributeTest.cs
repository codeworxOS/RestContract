using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Codeworx.Rest.UnitTests.Api.Contract;
using Codeworx.Rest.UnitTests.Data;
using Codeworx.Rest.UnitTests.Generated;
using Xunit;

namespace Codeworx.Rest.UnitTests
{
    public class RestProxyAttributeTest
    {
        [Fact]
        public void ExpectAllContractsToHaveARestProxyAttribute()
        {
            var contracts = typeof(IFileStreamController).Assembly.GetTypes()
                                .Where(p => p.GetCustomAttribute<RestRouteAttribute>() != null)
                                .ToList();

            IReadOnlyDictionary<Type, Type> proxies = typeof(FileStreamControllerClient).Assembly
                                                        .GetCustomAttributes<RestProxyAttribute>()
                                                        .ToImmutableDictionary(p => p.ContractType, p => p.ProxyType);

            Assert.All(contracts, p => Assert.True(p.IsAssignableFrom(Assert.Contains(p, proxies))));
        }
    }
}