using System.Threading.Tasks;
using Codeworx.Rest.UnitTests.Api.Contract;
using Codeworx.Rest.UnitTests.Data;
using Codeworx.Rest.UnitTests.Generated;
using Xunit;

namespace Codeworx.Rest.UnitTests
{
    public class PathTests: TestServerTestsBase
    {
        private readonly IPathController _controller;

        public PathTests()
        {
            _controller = new PathControllerClient(RestOptions);
        }

        [Fact]
        public async Task TestNoPathWithNoParameters()
        {
            var result = await _controller.EmptyPathWithoutParameters();
            Assert.True(result);
        }

        [Fact]
        public async Task TestNoPathWithQueryParameters()
        {
            var result = await _controller.EmptyPathWithQueryParameters(ItemsGenerator.TestString, ItemsGenerator.TestInt, ItemsGenerator.TestGuid, ItemsGenerator.TestDate);
            Assert.True(result);
        }

        [Fact]
        public async Task TestNoPathWithUrlParameters()
        {
            var result = await _controller.EmptyPathWithUrlParameters(ItemsGenerator.TestString, ItemsGenerator.TestInt, ItemsGenerator.TestGuid, ItemsGenerator.TestDate);
            Assert.True(result);
        }

        [Fact]
        public async Task TestNoPathWithBodyParameter()
        {
            var item = await ItemsGenerator.GenerateItem();
            var result = await _controller.EmptyPathWithBodyParameter(item);
            Assert.True(result);
        }

        [Fact]
        public async Task TestNoPathWithAllParameterTypes()
        {
            var item = await ItemsGenerator.GenerateItem();
            var result = await _controller.EmptyPathWithAllParameters(
                item,
                ItemsGenerator.TestString, ItemsGenerator.TestInt, ItemsGenerator.TestGuid, ItemsGenerator.TestDate,
                ItemsGenerator.TestString, ItemsGenerator.TestInt, ItemsGenerator.TestGuid, ItemsGenerator.TestDate);
            Assert.True(result);
        }

        [Fact]
        public async Task TestSimplePathWithNoParameters()
        {
            var result = await _controller.SimplePathWithoutParameters();
            Assert.True(result);
        }

        [Fact]
        public async Task TestSimplePathWithQueryParameters()
        {
            var result = await _controller.SimplePathWithQueryParameters(ItemsGenerator.TestString, ItemsGenerator.TestInt, ItemsGenerator.TestGuid, ItemsGenerator.TestDate);
            Assert.True(result);
        }

        [Fact]
        public async Task TestSimplePathWithUrlParameters()
        {
            var result = await _controller.SimplePathWithUrlParameters(ItemsGenerator.TestString, ItemsGenerator.TestInt, ItemsGenerator.TestGuid, ItemsGenerator.TestDate);
            Assert.True(result);
        }

        [Fact]
        public async Task TestSimplePathWithBodyParameter()
        {
            var item = await ItemsGenerator.GenerateItem();
            var result = await _controller.SimplePathWithBodyParameter(item);
            Assert.True(result);
        }

        [Fact]
        public async Task TestSimplePathWithAllParameterTypes()
        {
            var item = await ItemsGenerator.GenerateItem();
            var result = await _controller.SimplePathWithAllParameters(
                item,
                ItemsGenerator.TestString, ItemsGenerator.TestInt, ItemsGenerator.TestGuid, ItemsGenerator.TestDate,
                ItemsGenerator.TestString, ItemsGenerator.TestInt, ItemsGenerator.TestGuid, ItemsGenerator.TestDate);
            Assert.True(result);
        }

        [Fact]
        public async Task TestComplexPathWithNoParameters()
        {
            var result = await _controller.ComplexPathWithoutParameters();
            Assert.True(result);
        }


        [Fact]
        public async Task TestComplexPathWithQueryParameters()
        {
            var result = await _controller.ComplexPathWithQueryParameters(ItemsGenerator.TestString, ItemsGenerator.TestInt, ItemsGenerator.TestGuid, ItemsGenerator.TestDate);
            Assert.True(result);
        }

        [Fact]
        public async Task TestComplexPathWithUrlParameters()
        {
            var result = await _controller.ComplexPathWithUrlParameters(ItemsGenerator.TestString, ItemsGenerator.TestInt, ItemsGenerator.TestGuid, ItemsGenerator.TestDate);
            Assert.True(result);
        }

        [Fact]
        public async Task TestComplexPathWithBodyParameter()
        {
            var item = await ItemsGenerator.GenerateItem();
            var result = await _controller.ComplexPathWithBodyParameter(item);
            Assert.True(result);
        }

        [Fact]
        public async Task TestComplexPathWithAllParameters()
        {
            var item = await ItemsGenerator.GenerateItem();
            var result = await _controller.ComplexPathWithAllParameters(
                item,
                ItemsGenerator.TestString, ItemsGenerator.TestInt, ItemsGenerator.TestGuid, ItemsGenerator.TestDate,
                ItemsGenerator.TestString, ItemsGenerator.TestInt, ItemsGenerator.TestGuid, ItemsGenerator.TestDate);
            Assert.True(result);
        }
    }
}
