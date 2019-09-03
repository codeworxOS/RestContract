using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Codeworx.Rest.UnitTests.Model;

namespace Codeworx.Rest.UnitTests.Api.Contract
{
    [RestRoute("api/SerializeResult")]
    public interface ISerializeResultController
    {
        [RestGet("void")]
        Task NoResult();

        [RestGet("string")]
        Task<string> StringResult();

        [RestGet("date")]
        Task<DateTime> DateTimeResult();

        [RestGet("date/nullable")]
        Task<DateTime?> NullableDateTimeResult();

        [RestGet("guid")]
        Task<Guid> GuidResult();

        [RestGet("guid/nullable")]
        Task<Guid?> NullableGuidResult();

        [RestGet("item")]
        Task<Item> ItemResult();

        [RestGet("item/null")]
        Task<Item> ItemNullResult();

        [RestGet("string/list")]
        Task<List<string>> StringListResult();

        [RestGet("date/list")]
        Task<List<DateTime>> DateTimeListResult();

        [RestGet("guid/list")]
        Task<List<Guid>> GuidListResult();

        [RestGet("item/list")]
        Task<List<Item>> ItemListResult();
    }
}
