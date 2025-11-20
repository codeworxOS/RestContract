using System.Threading.Tasks;

namespace Codeworx.Rest.UnitTests
{
    public interface INoneRestInterface
    {
        Task<bool> PublicMethodFromNoneRestInterfaceAsync();
    }
}
