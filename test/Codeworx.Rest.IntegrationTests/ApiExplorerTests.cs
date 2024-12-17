using System.Collections.Generic;
using System.Net;
using System.Text.Json;
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

        [Fact]
        public async Task TestIgnoreApiAsync()
        {
            var request = TestServer.CreateRequest("swagger/v1/swagger.json");
            var response = await request.SendAsync(HttpMethods.Get);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var swagger = await JsonSerializer.DeserializeAsync<Dictionary<string, object>>(await response.Content.ReadAsStreamAsync());
            var paths = (JsonElement)swagger["paths"];
            var hasProperty = paths.TryGetProperty("/api/ignoreapi/action1", out var value);
            Assert.False(hasProperty);

            hasProperty = paths.TryGetProperty("/api/ignoreapi/action2", out value);
            Assert.False(hasProperty);
        }

        [Fact]
        public async Task TestIgnoreApiActionAsync()
        {
            var request = TestServer.CreateRequest("swagger/v1/swagger.json");
            var response = await request.SendAsync(HttpMethods.Get);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var swagger = await JsonSerializer.DeserializeAsync<Dictionary<string, object>>(await response.Content.ReadAsStreamAsync());
            var paths = (JsonElement)swagger["paths"];
            var hasProperty = paths.TryGetProperty("/api/ignoreapiaction/notignored", out var value);

            Assert.True(hasProperty);

            hasProperty = paths.TryGetProperty("/api/ignoreapiaction/ignored", out value);

            Assert.False(hasProperty);
        }

    }
}
