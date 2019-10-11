using System.Threading.Tasks;

namespace Codeworx.Rest.UnitTests.Api.Contract
{
    [RestRoute("api/Inheritance")]
    public interface IDerivedController : IBaseController
    {
        [RestGet("derived")]
        Task<string> Derived();
    }
}
