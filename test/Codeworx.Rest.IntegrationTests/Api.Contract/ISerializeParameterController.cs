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
        [RestGet("string/{parameter}")]
        Task<string> GetStringUrlParameter(string parameter);
        [RestPut("string")]
        Task<string> GetStringBodyParameter([BodyMember]string parameter);

        [RestGet("date")]
        Task<DateTime> GetDateTimeQueryParameter(DateTime parameter);
        [RestGet("date/{parameter}")]
        Task<DateTime> GetDateTimeUrlParameter(DateTime parameter);
        [RestPut("date")]
        Task<DateTime> GetDateTimeBodyParameter([BodyMember]DateTime parameter);

        [RestGet("guid")]
        Task<Guid> GetGuidQueryParameter(Guid parameter);
        [RestGet("guid/{parameter}")]
        Task<Guid> GetGuidUrlParameter(Guid parameter);
        [RestPut("guid")]
        Task<Guid> GetGuidBodyParameter([BodyMember]Guid parameter);

        [RestGet("int")]
        Task<int> GetIntQueryParameter(int parameter);
        [RestGet("int/{parameter}")]
        Task<int> GetIntUrlParameter(int parameter);
        [RestPut("int")]
        Task<int> GetIntBodyParameter([BodyMember]int parameter);

        [RestGet("guid/list")]
        Task<List<Guid>> GetGuidListQueryParameter(List<Guid> parameter);
        [RestGet("guid/list/{parameter}")]
        Task<List<Guid>> GetGuidListUrlParameter(List<Guid> parameter);
        [RestPut("guid/list")]
        Task<List<Guid>> GetGuidListBodyParameter([BodyMember]List<Guid> parameter);

        [RestPut("item")]
        Task<Item> GetItemBodyParameter([BodyMember]Item parameter);
        [RestPut("item")]
        Task<Item> GetItemBodyParameterWithNoAttribute(Item parameter);
        [RestPut("item")]
        Task<Item> GetItemBodyParameterAsLastParameter(Guid id, string name, [BodyMember] Item parameter);
    }
}
