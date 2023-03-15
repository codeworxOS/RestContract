using System;
using System.Threading.Tasks;
using Codeworx.Rest.Client;
using Codeworx.Rest.UnitTests.Api.Contract;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Codeworx.Rest.UnitTests
{
    public class GroupedServiceTest
    {
        public class InheritanceTest
        {
            public InheritanceTest()
            {
            }

            [Fact]
            public async Task SetBaseUrlForGroup()
            {
                await Task.Yield();

                var services = new ServiceCollection();
                services.AddRestClient()
                    .WithBaseUrl("http://localhost:1234/testurl")
                    .Group(TestConstants.TestGroup, p => p.WithBaseUrl("http://localhost:4321/testurl"));

                var provider = services.BuildServiceProvider();

                var options = provider.GetRequiredService<RestOptions<IFileStreamController>>();
                var groupedOptions = provider.GetRequiredService<RestOptions<IGroupedService>>();
                Assert.Equal(new Uri("http://localhost:1234/testurl"), options.GetHttpClient().BaseAddress);
                Assert.Equal(new Uri("http://localhost:4321/testurl"), groupedOptions.GetHttpClient().BaseAddress);
            }

            [Fact]
            public async Task SetHttpClientForGroup()
            {
                await Task.Yield();

                var services = new ServiceCollection();
                services.AddSingleton<GroupInfo>(new GroupInfo { BaseUrl = "http://localhost:4321/testurl" });
                services.AddRestClient()
                    .WithBaseUrl("http://localhost:1234/testurl")
                    .Group(TestConstants.TestGroup,
                        p => p.WithHttpClient((sp, client) =>
                        {
                            var info = sp.GetRequiredService<GroupInfo>();
                            client.BaseAddress = new Uri(info.BaseUrl);
                        }));

                var provider = services.BuildServiceProvider();

                var options = provider.GetRequiredService<RestOptions<IFileStreamController>>();
                var groupedOptions = provider.GetRequiredService<RestOptions<IGroupedService>>();
                Assert.Equal(new Uri("http://localhost:1234/testurl"), options.GetHttpClient().BaseAddress);
                Assert.Equal(new Uri("http://localhost:4321/testurl"), groupedOptions.GetHttpClient().BaseAddress);
            }

            private class GroupInfo
            {
                public string BaseUrl { get; set; }
            }
        }
    }
}
