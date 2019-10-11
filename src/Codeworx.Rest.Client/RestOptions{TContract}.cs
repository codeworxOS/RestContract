using System.Collections.Generic;

namespace Codeworx.Rest.Client
{
    public class RestOptions<TContract> : RestOptions
        where TContract : class
    {
        public RestOptions(HttpClientFactory<TContract> clientFactory, DefaultFormatterSelector defaultFormatterSelector, IEnumerable<IContentFormatter> additionalFormatters)
            : base(() => clientFactory(), defaultFormatterSelector, additionalFormatters)
        {
        }
    }
}