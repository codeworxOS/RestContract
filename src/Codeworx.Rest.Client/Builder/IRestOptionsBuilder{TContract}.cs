using Microsoft.Extensions.DependencyInjection;

namespace Codeworx.Rest.Client
{
    public interface IRestOptionsBuilder<TContract>
        where TContract : class
    {
        IServiceCollection Services { get; }
    }
}