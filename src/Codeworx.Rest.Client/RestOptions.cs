using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace Codeworx.Rest.Client
{
    public class RestOptions
    {
        private readonly HttpClientFactory _clientFactory;
        private readonly DefaultFormatterSelector _defaultFormatterSelector;
        private readonly IServiceErrorDispatcher _errorDispatcher;

        public RestOptions(HttpClientFactory clientFactory, DefaultFormatterSelector defaultFormatterSelector, IEnumerable<IContentFormatter> formatters, IEnumerable<IAdditionalDataProvider> additionalDataProviders, IServiceErrorDispatcher errorDispatcher)
        {
            AdditionalDataProviders = additionalDataProviders ?? Enumerable.Empty<IAdditionalDataProvider>();
            this._errorDispatcher = errorDispatcher;
            _clientFactory = clientFactory;
            _defaultFormatterSelector = defaultFormatterSelector;
            Formatters = formatters;
        }

        public string DefaultFormatter => _defaultFormatterSelector();

        public IServiceErrorDispatcher ErrorDispatcher => _errorDispatcher;

        public IEnumerable<IAdditionalDataProvider> AdditionalDataProviders { get; }

        public IEnumerable<IContentFormatter> Formatters { get; }

        public virtual IContentFormatter GetFormatter(string mimeType = null)
        {
            return Formatters.FirstOrDefault(p => p.MimeType.Equals(mimeType, System.StringComparison.OrdinalIgnoreCase)) ?? Formatters.FirstOrDefault(p => p.MimeType.Equals(DefaultFormatter, System.StringComparison.OrdinalIgnoreCase));
        }

        public IReadOnlyDictionary<string, object> GetAdditionData()
        {
            var temp = new ConcurrentDictionary<string, List<object>>();

            foreach (var item in AdditionalDataProviders)
            {
                foreach (var data in item.GetValues())
                {
                    var values = temp.GetOrAdd(data.Key, p => new List<object>());
                    values.Add(data.Value);
                }
            }

            var result = temp.ToDictionary(p => p.Key, p => p.Value.Count > 1 ? p.Value : p.Value.FirstOrDefault());

            return result;
        }

        public virtual HttpClient GetHttpClient()
        {
            return _clientFactory();
        }
    }
}