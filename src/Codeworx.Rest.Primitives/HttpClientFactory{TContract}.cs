﻿using System.Net.Http;

namespace Codeworx.Rest
{
    public delegate HttpClient HttpClientFactory<TContract>()
        where TContract : class;
}