using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Codeworx.Rest.Client;
using Codeworx.Rest.UnitTests.Api.Contract;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Codeworx.Rest.UnitTests
{
    public class HttpClientFactoryTest
    {
        [Fact]
        public async Task CheckHttpClientFactoryRegistrationWithBaseUrlAsync()
        {
            HttpClient c1, c2, c3, c4;

            var defaultCallCount = 0;
            var pathCallCount = 0;

            var services = new ServiceCollection();
            services.AddRestClient()
                .WithHttpClient((sp, client) =>
                {
                    defaultCallCount++;
                    client.BaseAddress = new Uri("http://localhost:1234/");
                })
                .Contract<IPathService>(p =>
                    p.WithHttpClient((sp, client) =>
                    {
                        pathCallCount++;
                        client.BaseAddress = new Uri("http://localhost:4321/");
                    })
                );

            using (var sp = services.BuildServiceProvider())
            {
                using (var scope = sp.CreateScope())
                {
                    var statusCodeClient = scope.ServiceProvider.GetService<RestClient<IStatusCodeController>>();
                    c1 = statusCodeClient.Options.GetHttpClient();
                    var pathService = scope.ServiceProvider.GetService<RestClient<IPathService>>();
                    c2 = pathService.Options.GetHttpClient();
                }

                using (var scope = sp.CreateScope())
                {
                    var statusCodeClient = scope.ServiceProvider.GetService<RestClient<IStatusCodeController>>();
                    c3 = statusCodeClient.Options.GetHttpClient();
                    var pathService = scope.ServiceProvider.GetService<RestClient<IPathService>>();
                    c4 = pathService.Options.GetHttpClient();
                }
            }

            Assert.Equal(new Uri("http://localhost:1234/"), c1.BaseAddress);
            Assert.Equal(new Uri("http://localhost:1234/"), c3.BaseAddress);

            Assert.Equal(new Uri("http://localhost:4321/"), c2.BaseAddress);
            Assert.Equal(new Uri("http://localhost:4321/"), c4.BaseAddress);
        }
    }
}