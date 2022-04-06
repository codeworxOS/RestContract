using System;
using System.Threading;
using System.Threading.Tasks;
using Codeworx.Rest.UnitTests.Api.Contract;
using Xunit;

namespace Codeworx.Rest.UnitTests
{
    public class CancellationHandlingTest : TestServerTestsBase
    {
        private readonly ICancellationTokenController _controller;

        public CancellationHandlingTest()
        {
            _controller = Client<ICancellationTokenController>(FormatterSelection.Protobuf);
        }

        [Fact]
        public async Task TestHttpDelete()
        {
            var tokenSource = new CancellationTokenSource();
            var isCancelled = await _controller.Delete(tokenSource.Token);
            Assert.False(isCancelled);
            tokenSource.Cancel();
            await ThrowsCanceledAsync(async () => await _controller.Delete(tokenSource.Token));
        }

        [Fact]
        public async Task TestHttpGet()
        {
            var tokenSource = new CancellationTokenSource();
            var isCancelled = await _controller.Get(tokenSource.Token);
            Assert.False(isCancelled);
            tokenSource.Cancel();
            await ThrowsCanceledAsync(async () => await _controller.Get(tokenSource.Token));
        }


        [Fact]
        public async Task TestMultipleCancellationTokenParameters_Expects_Exception()
        {
            await Assert.ThrowsAsync<NotSupportedException>(async () =>
            {
                await _controller.GetWithMultipleCancellationToken(CancellationToken.None, CancellationToken.None);
            });
        }


        [Fact]
        public async Task TestHttpPost()
        {
            var tokenSource = new CancellationTokenSource();
            var isCancelled = await _controller.Post("test", tokenSource.Token);
            Assert.False(isCancelled);
            tokenSource.Cancel();
            await ThrowsCanceledAsync(async () => await _controller.Post("test", tokenSource.Token));
        }

        [Fact]
        public async Task TestHttpPut()
        {
            var tokenSource = new CancellationTokenSource();
            var isCancelled = await _controller.Put(new Model.Item { Id = Guid.NewGuid() }, tokenSource.Token);
            Assert.False(isCancelled);
            tokenSource.Cancel();

            await ThrowsCanceledAsync(async () => await _controller.Put(new Model.Item { Id = Guid.NewGuid() }, tokenSource.Token));

            await ThrowsCanceledAsync(async () => await _controller.Put(new Model.Item { Id = Guid.NewGuid() }, tokenSource.Token));
        }

        [Fact]
        public async Task TestHttpHead()
        {
            var tokenSource = new CancellationTokenSource();
            var isCancelled = await _controller.Head(tokenSource.Token);
            Assert.False(isCancelled);
            tokenSource.Cancel();
            await ThrowsCanceledAsync(async () => await _controller.Head(tokenSource.Token));
        }

        private static async Task ThrowsCanceledAsync(Func<Task> testCode)
        {
#if NET6_0_OR_GREATER
            await Assert.ThrowsAsync<TaskCanceledException>(testCode);
#else
            await Assert.ThrowsAsync<OperationCanceledException>(testCode);
#endif
        }
    }
}
