using System.Threading.Tasks;
using Codeworx.Rest.UnitTests.Api.Contract;
using Codeworx.Rest.UnitTests.Data;
using Codeworx.Rest.UnitTests.Generated;
using Codeworx.Rest.UnitTests.TestServerUtilities;
#if NETCOREAPP2_1
using Microsoft.AspNetCore.Mvc.Internal;
#endif
using Xunit;

namespace Codeworx.Rest.UnitTests
{
    public class MethodOverloadingTests : TestServerTestsBase<StartupNoExceptionMiddleware>
    {
        private readonly IMethodOverloadingController _controller;

        public MethodOverloadingTests()
        {
            _controller = Client<IMethodOverloadingController>(FormatterSelection.Json);
        }

        [Fact]
        public async Task TestMethodsWithSameNames()
        {
            var item = await _controller.MethodWithSameName();

            var expectedItem = await ItemsGenerator.GenerateItem();
            Assert.Equal(expectedItem, item);

            var resultItemName = "ResultItemName";
            var resultItem = await _controller.MethodWithSameName(resultItemName);
            Assert.Equal(resultItemName, resultItem.Name);
        }

#if NETCOREAPP2_1
        [Fact]
        public async Task TestMethodsWithSameUrls()
        {
            await Assert.ThrowsAsync<AmbiguousActionException>(async () => await _controller.MethodWithSameUrl1());
        }
#endif
    }
}