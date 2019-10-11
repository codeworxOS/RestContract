using Microsoft.Extensions.DependencyInjection;

namespace Codeworx.Rest
{
    public interface IRestOptionsBuilder
    {
        IServiceCollection Services { get; }
    }
}