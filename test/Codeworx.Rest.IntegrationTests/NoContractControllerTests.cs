using System.Net;
using System.Threading.Tasks;
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