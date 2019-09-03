using System.Threading.Tasks;
using Codeworx.Rest.UnitTests.Api.Contract;

namespace Codeworx.Rest.UnitTests.Api
{
    public class PublicMethodsWithNoInterfaceController : IPublicMethodsWithNoInterfaceController
    {
        public async Task<bool> MethodWithInterface()
        {
            return await Task.FromResult(true).ConfigureAwait(false);
        }

        //public async Task<bool> MethodWithNoInterface()
        //{
        //    return await Task.FromResult(true).ConfigureAwait(false);
        //}
    }
}
