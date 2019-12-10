using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Xunit;

namespace Codeworx.Rest.UnitTests
{
    public class ApiExplorerTests : TestServerTestsBase
    {
        [Fact]
        public async Task TestDownloadSwaggerFile()
        {
            var request = TestServer.CreateRequest("swagger/v1/swagger.json");
            var response = await request.SendAsync(HttpMethods.Get);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
