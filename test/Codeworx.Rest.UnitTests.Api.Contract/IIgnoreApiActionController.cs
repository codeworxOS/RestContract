using System.Threading.Tasks;

namespace Codeworx.Rest.UnitTests.Api.Contract
{
    [RestRoute("api/ignoreapiaction")]
    public interface IIgnoreApiActionController
    {
        [RestGet("notignored")]
        Task<bool> GetValueNotIgnoredAsync();

        [RestGet("ignored")]
        [IgnoreApi]
        Task<bool> GetValueIgnoredAsync();
    }
}
