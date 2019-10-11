using System.Threading.Tasks;
using Codeworx.Rest.UnitTests.Api.Contract;
using Xunit;

namespace Codeworx.Rest.UnitTests
{
    public class InheritanceTest : TestServerTestsBase
    {
        private readonly IDerivedController _controller;

        public InheritanceTest()
        {
            _controller = Client<IDerivedController>();
        }

        [Fact]
        public async Task TestMethodFromBaseController()
        {
            var actualItem = await _controller.Base();
            Assert.Equal(nameof(IDerivedController.Base), actualItem);
        }

        [Fact]
        public async Task TestMethodFromDerivedController()
        {
            var actualItem = await _controller.Derived();
            Assert.Equal(nameof(IDerivedController.Derived), actualItem);
        }
    }
}
