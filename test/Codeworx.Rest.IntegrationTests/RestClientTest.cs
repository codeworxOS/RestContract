using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Codeworx.Rest.UnitTests.Api.Contract;
using Microsoft.AspNetCore.TestHost;
using Xunit;

namespace Codeworx.Rest.UnitTests
{
    public class RestClientTest : TestServerTestsBase
    {
        private readonly IHttpOperationsController _controller;

        public RestClientTest()
        {
            _controller = Client<IHttpOperationsController>(FormatterSelection.Json);
        }

        protected override HttpClient CreateHttpClient(TestServer testServer)
        {
            var defaultHandler = testServer.CreateHandler();
            var preventEmptyBodyHandler = new PreventEmptyBodyHandler(defaultHandler);
            var httpClient = new HttpClient(preventEmptyBodyHandler)
            {
                BaseAddress = testServer.BaseAddress
            };
            return httpClient;
        }

        [Fact]
        public async Task TestPutMethodWithoutBody()
        {
            await _controller.Put();
        }

        [Fact]
        public async Task TestPostMethodWithoutBody()
        {
            await _controller.Post();
        }

        [Fact]
        public async Task TestDeleteMethodWithoutBody()
        {
            await _controller.Delete();
        }

        [Fact]
        public async Task TestGetMethodWithoutBody()
        {
            await _controller.Get();
        }

        [Fact]
        public async Task TestHeadMethodWithoutBody()
        {
            await _controller.Head();
        }
    }

    public class PreventEmptyBodyHandler : DelegatingHandler
    {
        public PreventEmptyBodyHandler(HttpMessageHandler innerHandler)
         : base(innerHandler)
        {
            
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (request.Method == HttpMethod.Put || request.Method == HttpMethod.Post)
            {
                if (request.Content == null)
                {
                    throw new ArgumentNullException($"method {request.Method} must have a request body");
                }
            }
            var response = await base.SendAsync(request, cancellationToken);
            return response;
        }
    }
}
