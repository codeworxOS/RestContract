using System;
using System.Threading.Tasks;
using Codeworx.Rest.UnitTests.Api.Contract;
using Codeworx.Rest.UnitTests.Generated;
using Xunit;

namespace Codeworx.Rest.UnitTests
{
    public class StatusCodeTests : TestServerTestsBase
    {
        private readonly IStatusCodeController _controller;

        public StatusCodeTests()
        {
            _controller = new StatusCodeControllerClient(RestOptions);
        }

        [Fact]
        public async Task TestSuccess()
        {
            var result = await _controller.GetValueSuccess();
            Assert.True(result);
        }

        [Fact]
        public async Task TestException()
        {
            await Assert.ThrowsAsync<InvalidOperationException>(async () => await _controller.GetValueException());
        }

    }
}
