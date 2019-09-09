using System.Threading.Tasks;
using Codeworx.Rest.UnitTests.Api.Contract;
using Codeworx.Rest.Client;

namespace Codeworx.Rest.UnitTests.Generated
{
    public class PublicMethodsWithNoInterfaceControllerClient : RestClient<IPublicMethodsWithNoInterfaceController>, IPublicMethodsWithNoInterfaceController
    {
        public PublicMethodsWithNoInterfaceControllerClient(RestOptions<IPublicMethodsWithNoInterfaceController> options): base(options)
        {
        }

        public PublicMethodsWithNoInterfaceControllerClient(RestOptions options): base(options)
        {
        }

        public Task<bool> MethodWithInterface()
        {
            return CallAsync(c => c.MethodWithInterface());
        }
    }
}