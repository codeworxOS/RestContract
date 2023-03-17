using Microsoft.Extensions.DependencyInjection;

namespace Codeworx.Rest
{
    public interface IGroupRestOptionsBuilder
    {
        string GroupId { get; }

        IServiceCollection Services { get; }
    }
}
