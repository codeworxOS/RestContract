using System.Threading.Tasks;
using Codeworx.Rest.UnitTests.Api.Contract;

namespace Codeworx.Rest.UnitTests.Api
{
    public class IgnoreApiActionController : IIgnoreApiActionController
    {
        public Task<bool> GetValueIgnoredAsync()
        {
            return Task.FromResult(true);
        }

        public Task<bool> GetValueNotIgnoredAsync()
        {
            return Task.FromResult(true);
        }
    }
}
