using System.Threading.Tasks;

namespace Codeworx.Rest.UnitTests.Api.Contract
{
    [RestRoute("api/grouped", TestConstants.TestGroup)]
    public interface IGroupedService
    {
        [RestGet()]
        Task<bool> Call();
    }
}