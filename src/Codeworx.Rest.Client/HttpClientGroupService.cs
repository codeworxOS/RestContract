using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace Codeworx.Rest.Client
{
    public class HttpClientGroupService
    {
        private readonly Dictionary<string, HttpClientGroup> _groups;
        private readonly IServiceProvider _serviceProvider;

        public HttpClientGroupService(IEnumerable<HttpClientGroup> groups, IServiceProvider serviceProvider)
        {
            _groups = groups.ToDictionary(p => p.GroupId, p => p);
            _serviceProvider = serviceProvider;
        }

        public bool TryGetClient(string groupId, out HttpClient httpClient)
        {
            if (_groups.TryGetValue(groupId, out var group))
            {
                httpClient = group.Factory(_serviceProvider);
                return true;
            }

            httpClient = null;
            return false;
        }
    }
}