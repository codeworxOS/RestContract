using System.Threading;
using System.Threading.Tasks;

namespace Codeworx.Rest.UnitTests.Api.Contract
{
    [RestRoute("api/CancellationToken")]
    public interface ICancellationTokenController
    {
        [RestDelete]
        Task<bool> Delete(CancellationToken token);

        [RestGet]
        Task<bool> Get(CancellationToken token);

        [RestPost]
        Task<bool> Post(CancellationToken token);

        [RestPut]
        Task<bool> Put(CancellationToken token);

        [RestHead]
        Task<bool> Head(CancellationToken token);
    }
}
