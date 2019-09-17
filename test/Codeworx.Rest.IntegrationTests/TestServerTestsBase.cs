using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Codeworx.Rest.Client;
using Codeworx.Rest.UnitTests.Generated;
using Codeworx.Rest.UnitTests.TestServerUtilities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;

namespace Codeworx.Rest.UnitTests
{
    public abstract class TestServerTestsBase : TestServerTestsBase<Startup>
    {
    }

    public abstract class TestServerTestsBase<TStartup> : IDisposable
        where TStartup : class
    {
        private ServiceProvider _clientProvider;
        private HttpClient _httpClient;
        private TestServer _testServer;

        public TestServerTestsBase()
        {
            var builder = new WebHostBuilder()
                .UseStartup<TStartup>();
            _testServer = new TestServer(builder);

            _httpClient = _testServer.CreateClient();

            var services = new ServiceCollection();
            services.AddRestClient()
                .WithHttpClient(p => _httpClient)
                .AddRestProxies(typeof(FileStreamControllerClient).Assembly);

            _clientProvider = services.BuildServiceProvider();
        }

        public virtual void Dispose()
        {
            _clientProvider.Dispose();
            _httpClient?.Dispose();
            _testServer?.Dispose();
        }

        protected TContract Client<TContract>() => _clientProvider.GetRequiredService<TContract>();
    }
}