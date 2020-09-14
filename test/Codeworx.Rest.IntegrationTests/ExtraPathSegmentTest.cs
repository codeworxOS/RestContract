using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Codeworx.Rest.UnitTests.Api.Contract;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Codeworx.Rest.UnitTests
{
    public class ExtraPathSegmentTest : TestServerTestsBase
    {
        public ExtraPathSegmentTest()
        {
        }

        [Fact]
        public async Task TestAdditionalConstantData()
        {
            var client = Client<IExtraPathSegmentController>(FormatterSelection.Json,
                            new Dictionary<string, object> { { "tenant", "sample" } });

            var response = await client.GetAsync("abc");

            Assert.Equal($"sample-abc", response);
        }

        [Fact]
        public async Task TestAdditionalServiceProviderData()
        {
            var sp = GetClientProvider(FormatterSelection.Json, service =>
            {
                var data = service.GetRequiredService<ExtraData>();
                return new Dictionary<string, object> { { "tenant", data.Tenant } };
            });

            using (var scope1 = sp.CreateScope())
            {
                var extra = scope1.ServiceProvider.GetRequiredService<ExtraData>();
                extra.Tenant = "sample1";

                var client = scope1.ServiceProvider.GetRequiredService<IExtraPathSegmentController>();
                var response = await client.GetAsync("abc");

                Assert.Equal($"sample1-abc", response);
            }

            using (var scope2 = sp.CreateScope())
            {
                var extra = scope2.ServiceProvider.GetRequiredService<ExtraData>();
                extra.Tenant = "sample2";

                var client = scope2.ServiceProvider.GetRequiredService<IExtraPathSegmentController>();
                var response = await client.GetAsync("abc");

                Assert.Equal($"sample2-abc", response);
            }

        }

        protected override ServiceCollection BuildServiceCollection(FormatterSelection selection, Action<IRestOptionsBuilder> builderAction = null)
        {
            var result = base.BuildServiceCollection(selection, builderAction);
            result.AddScoped<ExtraData>();

            return result;
        }

        public class ExtraData
        {
            public string Tenant { get; set; }
        }
    }
}
