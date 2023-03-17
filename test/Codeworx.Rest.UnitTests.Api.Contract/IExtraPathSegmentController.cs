using System.Threading.Tasks;

namespace Codeworx.Rest.UnitTests.Api.Contract
{
    [RestRoute("{tenant}/api/authorized")]
    public interface IExtraPathSegmentController
    {
        [RestGet("{id}")]
        Task<string> GetAsync(string id);
    }
}
