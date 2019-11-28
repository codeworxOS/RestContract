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
        private HashSet<IDisposable> _clientProviders;
        private HttpClient _httpClient;
        private TestServer _testServer;

        public TestServerTestsBase()
        {
            _clientProviders = new HashSet<IDisposable>();
            var builder = new WebHostBuilder()
                .UseStartup<TStartup>();
            _testServer = new TestServer(builder);

            _httpClient = _testServer.CreateClient();
        }

        protected TestServer TestServer => _testServer;

        public virtual void Dispose()
        {
            foreach (var disp in _clientProviders)
            {
                disp.Dispose();
            }

            _httpClient?.Dispose();
            _testServer?.Dispose();
        }

        protected TContract Client<TContract>(FormatterSelection selection)
        {
            var provider = GetClientProvider(selection);
            _clientProviders.Add(provider);

            return provider.GetRequiredService<TContract>();
        }

        private ServiceProvider GetClientProvider(FormatterSelection selection)
        {
            var services = new ServiceCollection();
            var builder = services.AddRestClient()
                .WithTestingHttpClient(p => _httpClient)
                .AddRestProxies(typeof(FileStreamControllerClient).Assembly);

            if (selection == FormatterSelection.Protobuf)
            {
                builder = builder.AddProtobufFormatter(makeDefault: true);
            }

            return services.BuildServiceProvider();
        }
    }
}