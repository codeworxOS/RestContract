using System.Threading.Tasks;
using Codeworx.Rest.UnitTests.Api.Contract;
using Codeworx.Rest.UnitTests.Api.Contract.Model;
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

        [Fact]
        public async Task TestResponseTypes()
        {
            await Assert.ThrowsAsync<ServiceException<EntryNotFoundError>>(async () => await _controller.DeleteEntry("1"));

            var error = await Assert.ThrowsAsync<ServiceException<StillInUseError>>(async () => await _controller.DeleteEntry("2"));

            Assert.Equal("Invoice", error.Payload.BlockingResource);

            await _controller.DeleteEntry("3");
        }
    }
}