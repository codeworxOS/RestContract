using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using Codeworx.Rest.Client;
using Codeworx.Rest.UnitTests.Api.Contract;
using Codeworx.Rest.UnitTests.Data;
using Xunit;

namespace Codeworx.Rest.UnitTests
{
    public class MimeTypeTests : TestServerTestsBase
    {
        private readonly IMimeTypeController _controller;
        private readonly string _testFileName;

        public MimeTypeTests()
        {
            _controller = Client<IMimeTypeController>(FormatterSelection.Json);
            _testFileName = ItemsGenerator.CreateTestFilename();
        }

        public override void Dispose()
        {
            base.Dispose();
            ItemsGenerator.DeleteFile(_testFileName);
        }

        [Fact()]
        public async Task TestPlainStream()
        {
            using var client = this.TestServer.CreateClient();

            var response = await client.GetAsync("/api/mimetypes/plain-stream");

            Assert.Equal("text/plain", response.Content.Headers.ContentType.MediaType);

            var content = await response.Content.ReadAsStringAsync();

            Assert.Equal("test", content);
        }

        [Fact()]
        public async Task TestPutTextWithApiAsync()
        {
            var responseContent = await _controller.PutText("testchen");

            Assert.Equal("testchen", responseContent);
        }


        [Fact()]
        public async Task TestPutTextAsync()
        {
            using var client = this.TestServer.CreateClient();

            var content = new StringContent("testchen");

            var response = await client.PutAsync("/api/mimetypes", content);

            var responseContent = await response.Content.ReadAsStringAsync();

            Assert.Equal("testchen", responseContent);
        }

        [Fact()]
        public async Task CheckSwaggerResponseTypesAsync()
        {
            using var client = this.TestServer.CreateClient();
            var response = await client.GetAsync("swagger/v1/swagger.json");
            var swagger = await JsonSerializer.DeserializeAsync<Dictionary<string, object>>(await response.Content.ReadAsStreamAsync());

            var paths = (JsonElement)swagger["paths"];

            var responses = paths.GetProperty("/api/mimetypes/pdf").GetProperty("get").GetProperty("responses");
            Assert.True(responses.TryGetProperty("200", out var response1));
            Assert.Single(response1.GetProperty("content").EnumerateObject());
            Assert.True(response1.GetProperty("content").TryGetProperty("application/pdf", out var pdf));

            Assert.True(responses.TryGetProperty("400", out var response2));
            Assert.Single(response2.GetProperty("content").EnumerateObject());
            Assert.True(response2.GetProperty("content").TryGetProperty("text/plain", out var text));

        }


        [Fact()]
        public async Task CheckSwaggerBodyTypesAsync()
        {
            using var client = this.TestServer.CreateClient();
            var response = await client.GetAsync("swagger/v1/swagger.json");
            var swagger = await JsonSerializer.DeserializeAsync<Dictionary<string, object>>(await response.Content.ReadAsStreamAsync());

            var paths = (JsonElement)swagger["paths"];

            var content = paths.GetProperty("/api/mimetypes/uri")
                .GetProperty("put")
                .GetProperty("requestBody")
                .GetProperty("content");

            Assert.Equal(3, content.EnumerateObject().Count());


            Assert.True(content.TryGetProperty("application/json", out var body1));
            Assert.True(content.TryGetProperty("text/plain", out var body2));
            Assert.True(content.TryGetProperty("application/*+json", out var body3));
        }

        [Fact()]
        public async Task TestPutUriAsync()
        {
            using var client = this.TestServer.CreateClient();

            var content = new StringContent("http://www.google.com");
            var response = await client.PutAsync("/api/mimetypes/uri", content);
            var responseContent = await response.Content.ReadAsStringAsync();

            Assert.Equal("http://www.google.com/", responseContent);

            var json = JsonContent.Create(new Uri("http://www.google.com"));
            response = await client.PutAsync("/api/mimetypes/uri", json);
            responseContent = await response.Content.ReadAsStringAsync();

            Assert.Equal("http://www.google.com/", responseContent);
        }


        [Fact()]
        public async Task TestPlainString()
        {
            using var client = this.TestServer.CreateClient();

            var response = await client.GetAsync("/api/mimetypes/plain");

            Assert.Equal("text/plain", response.Content.Headers.ContentType.MediaType);

            var content = await response.Content.ReadAsStringAsync();

            Assert.Equal("test", content);
        }

        [Fact()]
        public async Task TestPdf()
        {
            using var client = this.TestServer.CreateClient();

            var response = await client.GetAsync("/api/mimetypes/pdf");

            Assert.Equal("application/pdf", response.Content.Headers.ContentType.MediaType);
        }


        [Fact()]
        public async Task TestOctet()
        {
            using var client = this.TestServer.CreateClient();

            var response = await client.GetAsync("/api/mimetypes/octet");

            Assert.Equal("application/octet-stream", response.Content.Headers.ContentType.MediaType);
        }
    }
}