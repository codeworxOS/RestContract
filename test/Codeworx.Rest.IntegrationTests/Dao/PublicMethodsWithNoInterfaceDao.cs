using System.Threading.Tasks;
using Codeworx.Rest.Client;
using Codeworx.Rest.UnitTests.Api.Contract;

namespace Codeworx.Rest.UnitTests.Dao
{
    public class PublicMethodsWithNoInterfaceDao : RestClient<IPublicMethodsWithNoInterfaceController>, IPublicMethodsWithNoInterfaceController
    {
        public PublicMethodsWithNoInterfaceDao(RestOptions options)
            : base(options)
        {
        }

        public Task<bool> MethodWithInterface()
        {
            return CallAsync(c => c.MethodWithInterface());
        }
    }
}
