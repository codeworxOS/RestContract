using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace Codeworx.Rest.Client
{
    public class RestOptions
    {
        private readonly HttpClientFactory _clientFactory;
        private readonly DefaultFormatterSelector _defaultFormatterSelector;

        public RestOptions(HttpClientFactory clientFactory, DefaultFormatterSelector defaultFormatterSelector, IEnumerable<IContentFormatter> formatters)
        {
            _clientFactory = clientFactory;
            _defaultFormatterSelector = defaultFormatterSelector;
            Formatters = formatters;
        }

        public string DefaultFormatter => _defaultFormatterSelector();

        public IEnumerable<IContentFormatter> Formatters { get; }

        public virtual IContentFormatter GetFormatter(string mimeType = null)
        {
            return Formatters.FirstOrDefault(p => p.MimeType.Equals(mimeType, System.StringComparison.OrdinalIgnoreCase)) ?? Formatters.FirstOrDefault(p => p.MimeType.Equals(DefaultFormatter, System.StringComparison.OrdinalIgnoreCase));
        }

        public virtual HttpClient GetHttpClient()
        {
            return _clientFactory();
        }
    }
}