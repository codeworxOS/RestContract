using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Codeworx.Rest.Client;
using Codeworx.Rest.UnitTests.Api.Contract;
using Codeworx.Rest.UnitTests.Model;

namespace Codeworx.Rest.UnitTests.Dao
{
    public class SerializeResultDao : RestClient<ISerializeResultController>, ISerializeResultController
    {
        public SerializeResultDao(RestOptions options)
            : base(options)
        {
        }

        public Task NoResult()
        {
            return CallAsync(c => c.NoResult());
        }

        public Task<string> StringResult()
        {
            return CallAsync(c => c.StringResult());
        }

        public Task<DateTime> DateTimeResult()
        {
            return CallAsync(c => c.DateTimeResult());
        }

        public Task<DateTime?> NullableDateTimeResult()
        {
            return CallAsync(c => c.NullableDateTimeResult());
        }

        public Task<Guid> GuidResult()
        {
            return CallAsync(c => c.GuidResult());
        }

        public Task<Guid?> NullableGuidResult()
        {
            return CallAsync(c => c.NullableGuidResult());
        }

        public Task<Item> ItemResult()
        {
            return CallAsync(c => c.ItemResult());
        }

        public Task<Item> ItemNullResult()
        {
            return CallAsync(c => c.ItemNullResult());
        }

        public Task<List<string>> StringListResult()
        {
            return CallAsync(c => c.StringListResult());
        }

        public Task<List<DateTime>> DateTimeListResult()
        {
            return CallAsync(c => c.DateTimeListResult());
        }

        public Task<List<Guid>> GuidListResult()
        {
            return CallAsync(c => c.GuidListResult());
        }

        public Task<List<Item>> ItemListResult()
        {
            return CallAsync(c => c.ItemListResult());
        }
    }
}
