using System.Threading.Tasks;

namespace Codeworx.Rest.UnitTests.Api.Contract
{
    [RestRoute("api/ignoreapi")]
    [IgnoreApi]
    public interface IIgnoreApiController
    {
        [RestGet("action1")]
        Task<bool> GetValue1Async();

        [RestGet("action2")]
        Task<bool> GetValue2Async();
    }
}
