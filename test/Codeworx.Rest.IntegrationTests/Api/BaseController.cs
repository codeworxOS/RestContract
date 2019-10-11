using System.Threading.Tasks;
using Codeworx.Rest.UnitTests.Api.Contract;

namespace Codeworx.Rest.UnitTests.Api
{
    public class BaseController : IBaseController
    {
        public Task<string> Base()
        {
            return Task.FromResult(nameof(IBaseController.Base));
        }
    }
}
