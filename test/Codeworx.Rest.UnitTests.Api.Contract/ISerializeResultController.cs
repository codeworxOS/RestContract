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
        [RestGet("date/list")]
        Task<List<DateTime>> DateTimeListResult();

        [RestGet("date")]
        Task<DateTime> DateTimeResult();

        [RestGet("decimal")]
        Task<decimal> DecimalResult();

        [RestGet("double")]
        Task<double> DoubleResult();

        [RestGet("float")]
        Task<float> FloatResult();

        [RestGet("guid/list")]
        Task<List<Guid>> GuidListResult();

        [RestGet("guid")]
        Task<Guid> GuidResult();

        [RestGet("int")]
        Task<int> IntResult();

        [RestGet("item/list")]
        Task<List<Item>> ItemListResult();

        [RestGet("item/null")]
        Task<Item> ItemNullResult();

        [RestGet("item")]
        Task<Item> ItemResult();

        [RestHead("void")]
        Task NoResult();

        [RestGet("date/nullable")]
        Task<DateTime?> NullableDateTimeResult();

        [RestGet("decimal/nullable")]
        Task<decimal?> NullableDecimalResult();

        [RestGet("double/nullable")]
        Task<double?> NullableDoubleResult();

        [RestGet("float/nullable")]
        Task<float?> NullableFloatResult();

        [RestGet("guid/nullable")]
        Task<Guid?> NullableGuidResult();

        [RestGet("int/nullable")]
        Task<int?> NullableIntResult();

        [RestGet("string/list")]
        Task<List<string>> StringListResult();

        [RestGet("string")]
        Task<string> StringResult();
    }
}