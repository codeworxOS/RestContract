using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Codeworx.Rest.Client;
using Codeworx.Rest.UnitTests.TestServerUtilities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;

namespace Codeworx.Rest.UnitTests
{
    public abstract class TestServerTestsBase : IDisposable
    {
        private TestServer _testServer;
        private HttpClient _httpClient;

        public TestServerTestsBase()
        {
            var builder = new WebHostBuilder()
                .UseStartup<Startup>();
            _testServer = new TestServer(builder);

            _httpClient = _testServer.CreateClient();
            RestOptions = new TestServerRestOptions(_httpClient);

        }

        protected RestOptions RestOptions { get; }


        public void Dispose()
        {
            _httpClient?.Dispose();
            _testServer?.Dispose();
        }
    }
}
