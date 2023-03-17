using System.Threading.Tasks;
using Codeworx.Rest.UnitTests.Api.Contract;

namespace Codeworx.Rest.UnitTests.Api
{
    public class GroupedService : IGroupedService
    {
        public Task<bool> Call()
        {
            return Task.FromResult(true);
        }
    }
}
