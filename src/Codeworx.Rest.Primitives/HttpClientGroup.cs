using System;
using System.Net.Http;

namespace Codeworx.Rest
{
    public class HttpClientGroup
    {
        public HttpClientGroup(string groupId, Func<IServiceProvider, HttpClient> factory)
        {
            GroupId = groupId;
            Factory = factory;
        }

        public string GroupId { get; }

        public Func<IServiceProvider, HttpClient> Factory { get; }
    }
}