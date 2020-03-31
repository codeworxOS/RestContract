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
        Task<bool> Post(string queryParameter, CancellationToken token);

        [RestPut]
        Task<bool> Put([BodyMember]UnitTests.Model.Item item, CancellationToken token);

        [RestHead]
        Task<bool> Head(CancellationToken token);

        [RestGet("multiplaTokens")]
        Task<bool> GetWithMultipleCancellationToken(CancellationToken token, CancellationToken token2);
    }
}
