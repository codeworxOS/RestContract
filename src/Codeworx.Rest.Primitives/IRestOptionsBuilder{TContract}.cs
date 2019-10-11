using Microsoft.Extensions.DependencyInjection;

namespace Codeworx.Rest
{
    public interface IRestOptionsBuilder<TContract>
        where TContract : class
    {
        IServiceCollection Services { get; }
    }
}