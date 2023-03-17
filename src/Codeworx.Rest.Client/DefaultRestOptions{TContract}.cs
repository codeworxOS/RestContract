using System.Net.Http;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Codeworx.Rest.Client
{
    public class DefaultRestOptions<TContract> : RestOptions<TContract>
        where TContract : class
    {
        private readonly HttpClientGroupService _service;

        public DefaultRestOptions(RestOptions options)
            : this(options, null)
        {
        }

        [ActivatorUtilitiesConstructor]
        public DefaultRestOptions(RestOptions options, HttpClientGroupService service)
            : base(() => options.GetHttpClient(), () => options.DefaultFormatter, options.Formatters, options.AdditionalDataProviders, options.ErrorDispatcher)
        {
            _service = service;
        }

        public override HttpClient GetHttpClient()
        {
            if (_service != null)
            {
                var route = typeof(TContract).GetCustomAttribute<RestRouteAttribute>();

                if (route != null && route.GroupId != null)
                {
                    if (_service.TryGetClient(route.GroupId, out var client))
                    {
                        return client;
                    }
                }
            }

            return base.GetHttpClient();
        }
    }
}