using System.Threading.Tasks;

namespace Codeworx.Rest.UnitTests.Api.Contract
{
    public interface IBaseController
    {
        [RestGet("base")]
        Task<string> Base();
    }
}
