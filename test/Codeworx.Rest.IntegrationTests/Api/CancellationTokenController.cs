using System.Threading;
using System.Threading.Tasks;
using Codeworx.Rest.UnitTests.Api.Contract;

namespace Codeworx.Rest.UnitTests.Api
{
    public class CancellationTokenController : ICancellationTokenController
    {
        public Task<bool> Delete(CancellationToken token)
        {
            return HandleToken(token);
        }

        public Task<bool> Get(CancellationToken token)
        {
            return HandleToken(token);
        }

        public Task<bool> GetWithMultipleCancellationToken(CancellationToken token, CancellationToken token2)
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> Head(CancellationToken token)
        {
            return HandleToken(token);
        }

        public Task<bool> Post(string queryParameter, CancellationToken token)
        {
            return HandleToken(token);
        }

        public Task<bool> Put(UnitTests.Model.Item item, CancellationToken token)
        {
            return HandleToken(token);
        }

        private Task<bool> HandleToken(CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            return Task.FromResult(token.IsCancellationRequested);
        }
    }
}
