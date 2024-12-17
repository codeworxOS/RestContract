using System.Threading.Tasks;
using Codeworx.Rest.UnitTests.Api.Contract;

namespace Codeworx.Rest.UnitTests.Api
{
    public class IgnoreApiController : IIgnoreApiController
    {
        public Task<bool> GetValue1Async()
        {
            return Task.FromResult(true);
        }

        public Task<bool> GetValue2Async()
        {
            return Task.FromResult(true);
        }
    }
}
