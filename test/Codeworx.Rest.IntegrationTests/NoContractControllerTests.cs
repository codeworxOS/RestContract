using System.IO;
using System.Net;
using System.Threading.Tasks;
using Codeworx.Rest.Client;
using Codeworx.Rest.UnitTests.Api.Contract;
using Codeworx.Rest.UnitTests.Api.Contract.Model;
using Codeworx.Rest.UnitTests.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using NSwag.Generation.AspNetCore;
using Xunit;

namespace Codeworx.Rest.UnitTests
{
    public class NoContractControllerTests : TestServerTestsBase
    {
        [Fact]
        public async Task TestNoContractControllerAnonymousMethod_Expects200()
        {
            var response = await HttpClient.GetAsync("api/nocontract/allow");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task TestNoContractControllerAnonymousMethod_Expects401()
        {
            var response = await HttpClient.GetAsync("api/noContract/deny");
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }
    }
}