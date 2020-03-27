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
            await Assert.ThrowsAsync<OperationCanceledException>(async () => await _controller.Delete(tokenSource.Token));
        }

        [Fact]
        public async Task TestHttpGet()
        {
            var tokenSource = new CancellationTokenSource();
            var isCancelled = await _controller.Get(tokenSource.Token);
            Assert.False(isCancelled);
            tokenSource.Cancel();
            await Assert.ThrowsAsync<OperationCanceledException>(async () => await _controller.Get(tokenSource.Token));
        }


        [Fact]
        public async Task TestHttpPost()
        {
            var tokenSource = new CancellationTokenSource();
            var isCancelled = await _controller.Post(tokenSource.Token);
            Assert.False(isCancelled);
            tokenSource.Cancel();
            await Assert.ThrowsAsync<OperationCanceledException>(async () => await _controller.Post(tokenSource.Token));
        }

        [Fact]
        public async Task TestHttpPut()
        {
            var tokenSource = new CancellationTokenSource();
            var isCancelled = await _controller.Put(tokenSource.Token);
            Assert.False(isCancelled);
            tokenSource.Cancel();
            await Assert.ThrowsAsync<OperationCanceledException>(async () => await _controller.Put(tokenSource.Token));
        }

        [Fact]
        public async Task TestHttpHead()
        {
            var tokenSource = new CancellationTokenSource();
            var isCancelled = await _controller.Head(tokenSource.Token);
            Assert.False(isCancelled);
            tokenSource.Cancel();
            await Assert.ThrowsAsync<OperationCanceledException>(async () => await _controller.Head(tokenSource.Token));
        }
    }
}
