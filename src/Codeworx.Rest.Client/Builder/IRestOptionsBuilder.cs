using Microsoft.Extensions.DependencyInjection;

namespace Codeworx.Rest.Client
{
    public interface IRestOptionsBuilder
    {
        IServiceCollection Services { get; }
    }
}