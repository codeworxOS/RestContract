using System.Threading.Tasks;
using Codeworx.Rest.UnitTests.Api.Contract;

namespace Codeworx.Rest.UnitTests.Api
{
    public class DerivedController : BaseController, IDerivedController
    {
        public Task<string> Derived()
        {
            return Task.FromResult(nameof(IDerivedController.Derived));
        }
    }
}