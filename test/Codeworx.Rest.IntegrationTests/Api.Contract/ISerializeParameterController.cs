using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Codeworx.Rest.UnitTests.Model;

namespace Codeworx.Rest.UnitTests.Api.Contract
{
    [RestRoute("api/SerializeParameter")]
    public interface ISerializeParameterController
    {
        [RestGet("string")]
        Task<string> GetStringQueryParameter(string parameter);
        [RestGet("string")]
        Task<string> GetStringUrlParameter(string parameter);
        [RestGet("string")]
        Task<string> GetStringBodyParameter(string parameter);

        [RestGet("date")]
        Task<DateTime> GetDateTimeQueryParameter(DateTime parameter);
        [RestGet("date")]
        Task<DateTime> GetDateTimeUrlParameter(DateTime parameter);
        [RestGet("date")]
        Task<DateTime> GetDateTimeBodyParameter(DateTime parameter);

        [RestGet("guid")]
        Task<Guid> GetGuidQueryParameter(Guid parameter);
        [RestGet("guid")]
        Task<Guid> GetGuidUrlParameter(Guid parameter);
        [RestGet("guid")]
        Task<Guid> GetGuidBodyParameter(Guid parameter);

        [RestGet("int")]
        Task<int> GetIntQueryParameter(int parameter);
        [RestGet("int")]
        Task<int> GetIntUrlParameter(int parameter);
        [RestGet("int")]
        Task<int> GetIntBodyParameter(int parameter);

        [RestGet("guid/list")]
        Task<List<Guid>> GetGuidListQueryParameter(List<Guid> parameter);
        [RestGet("guid/list")]
        Task<List<Guid>> GetGuidListUrlParameter(List<Guid> parameter);
        [RestGet("guid/list")]
        Task<List<Guid>> GetGuidListBodyParameter(List<Guid> parameter);

        [RestGet("item")]
        Task<Item> GetItemBodyParameter(Item parameter);
        [RestGet("item")]
        Task<Item> GetItemBodyParameterWithNoAttribute(Item parameter);
        [RestGet("item")]
        Task<Item> GetItemBodyParameterAsLastParameter(Guid id, string name, Item parameter);
    }
}
