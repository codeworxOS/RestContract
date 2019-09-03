using System.Threading.Tasks;

namespace Codeworx.Rest.UnitTests.Api.Contract
{
    [RestRoute("api/NoInterface")]
    public interface IPublicMethodsWithNoInterfaceController
    {
        [RestGet]
        Task<bool> MethodWithInterface();
    }
}
