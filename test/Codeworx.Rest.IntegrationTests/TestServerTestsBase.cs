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
    public abstract class TestServerTestsBase : TestServerTestsBase<Startup>
    {
    }

    public abstract class TestServerTestsBase<TStartup> : IDisposable
        where TStartup : class
    {
        private HttpClient _httpClient;
        private TestServer _testServer;

        public TestServerTestsBase()
        {
            var builder = new WebHostBuilder()
                .UseStartup<TStartup>();
            _testServer = new TestServer(builder);

            _httpClient = _testServer.CreateClient();
            RestOptions = new TestServerRestOptions(_httpClient);
        }

        protected RestOptions RestOptions { get; }

        public virtual void Dispose()
        {
            _httpClient?.Dispose();
            _testServer?.Dispose();
        }
    }
}