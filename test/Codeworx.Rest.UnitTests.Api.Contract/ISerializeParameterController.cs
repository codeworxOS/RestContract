using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Codeworx.Rest.UnitTests.Model;

namespace Codeworx.Rest.UnitTests.Api.Contract
{
    [RestRoute("api/SerializeParameter")]
    public interface ISerializeParameterController
    {
        [RestPut("date/body")]
        Task<DateTime?> GetDateTimeBodyParameter([BodyMember]DateTime? parameter);

        [RestGet("date/query")]
        Task<DateTime?> GetDateTimeQueryParameter(DateTime? parameter);

        [RestGet("date/url/{parameter?}")]
        Task<DateTime?> GetDateTimeUrlParameter(DateTime? parameter);

        [RestPut("guid/body")]
        Task<Guid?> GetGuidBodyParameter([BodyMember]Guid? parameter);

        [RestPut("guid/list/body")]
        Task<List<Guid>> GetGuidListBodyParameter([BodyMember]List<Guid> parameter);

        [RestGet("guid/list/query")]
        Task<List<Guid>> GetGuidListQueryParameter(List<Guid> parameter);

        [RestGet("guid/list/url/{parameter}")]
        Task<List<Guid>> GetGuidListUrlParameter(List<Guid> parameter);

        [RestGet("guid/query")]
        Task<Guid?> GetGuidQueryParameter(Guid? parameter);

        [RestGet("guid/url/{parameter?}")]
        Task<Guid?> GetGuidUrlParameter(Guid? parameter);

        [RestPut("int/body")]
        Task<int?> GetIntBodyParameter([BodyMember]int? parameter);

        [RestGet("int/query")]
        Task<int?> GetIntQueryParameter(int? parameter);

        [RestGet("int/url/{parameter?}")]
        Task<int?> GetIntUrlParameter(int? parameter);

        [RestPut("item/body")]
        Task<Item> GetItemBodyParameter([BodyMember]Item parameter);

        [RestPut("item/body/lastParameter")]
        Task<Item> GetItemBodyParameterAsLastParameter(Guid id, string name, [BodyMember] Item parameter);

        [RestPut("item/body/noAttribute")]
        Task<Item> GetItemBodyParameterWithNoAttribute(Item parameter);

        [RestPut("string/body")]
        Task<string> GetStringBodyParameter([BodyMember]string parameter);

        [RestGet("string/query")]
        Task<string> GetStringQueryParameter(string parameter);

        [RestGet("string/url/{parameter?}")]
        Task<string> GetStringUrlParameter(string parameter);
    }
}