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

        public Task<bool> Head(CancellationToken token)
        {
            return HandleToken(token);
        }

        public Task<bool> Post(CancellationToken token)
        {
            return HandleToken(token);
        }

        public Task<bool> Put(CancellationToken token)
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
