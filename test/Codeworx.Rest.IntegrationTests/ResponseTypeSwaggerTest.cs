using System.Collections.Generic;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Xunit;

namespace Codeworx.Rest.UnitTests
{
    public class ResponseTypeSwaggerTest : TestServerTestsBase
    {
        [Fact]
        public async Task TestDownloadSwaggerFileHasResponseTypeData()
        {
            var request = TestServer.CreateRequest("swagger/v1/swagger.json");
            var response = await request.SendAsync(HttpMethods.Get);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var swagger = await JsonSerializer.DeserializeAsync<Dictionary<string, object>>(
                await response.Content.ReadAsStreamAsync());

            var paths = (JsonElement)swagger["paths"];

            var responses = paths.GetProperty("/api/StatusCode/Delete").GetProperty("get").GetProperty("responses");

            Assert.True(responses.TryGetProperty("200", out var response1));

            Assert.True(responses.TryGetProperty("409", out var response2));

            Assert.True(responses.TryGetProperty("410", out var response3));

        }

    }
}
