using System.Threading.Tasks;
using Codeworx.Rest.UnitTests.Api.Contract;
using Xunit;

namespace Codeworx.Rest.UnitTests
{
    public class StatusCodeTests : TestServerTestsBase
    {
        private readonly IStatusCodeController _controller;

        public StatusCodeTests()
        {
            _controller = Client<IStatusCodeController>(FormatterSelection.Json);
        }

        [Fact]
        public async Task TestException()
        {
            await Assert.ThrowsAsync<UnexpectedHttpStatusCodeException>(async () => await _controller.GetValueException());
        }

        [Fact]
        public async Task TestSuccess()
        {
            var result = await _controller.GetValueSuccess();
            Assert.True(result);
        }
    }
}