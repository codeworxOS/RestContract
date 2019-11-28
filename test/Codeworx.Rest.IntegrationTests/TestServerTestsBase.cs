using System;
using System.Collections.Generic;
using System.Net.Http;
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
        private readonly HashSet<IDisposable> _clientProviders;
        private readonly HttpClient _httpClient;
        private readonly TestServer _testServer;

        protected TestServerTestsBase()
        {
            _clientProviders = new HashSet<IDisposable>();
            var builder = new WebHostBuilder()
                .UseStartup<TStartup>();
            _testServer = new TestServer(builder);

            _httpClient = CreateHttpClient(_testServer);
            
        }

        protected virtual HttpClient CreateHttpClient(TestServer testServer)
        {
            return testServer.CreateClient();
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