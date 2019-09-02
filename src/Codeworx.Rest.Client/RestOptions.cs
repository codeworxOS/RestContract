using System;
using System.Net.Http;

namespace Codeworx.Rest.Client
{
    public class RestOptions
    {
        public RestOptions(string baseUrl)
        {
            BaseUrl = baseUrl;
        }

        public string BaseUrl { get; }

        public HttpClient GetHttpClient()
        {
            return new HttpClient { BaseAddress = new Uri(BaseUrl) };
        }
    }
}