using System.Threading.Tasks;
using Codeworx.Rest.UnitTests.Api.Contract;
using Codeworx.Rest.UnitTests.Data;
using Codeworx.Rest.UnitTests.Generated;
using Xunit;

namespace Codeworx.Rest.UnitTests
{
    public class PathTests : TestServerTestsBase
    {
        private readonly IPathService _service;

        public PathTests()
        {
            _service = Client<IPathService>(FormatterSelection.Json);
        }

        [Fact]
        public async Task TestComplexPathWithAllParameters()
        {
            var item = await ItemsGenerator.GenerateItem();
            var result = await _service.ComplexPathWithAllParameters(
                item,
                ItemsGenerator.TestString, ItemsGenerator.TestInt, ItemsGenerator.TestGuid, ItemsGenerator.TestDate,
                ItemsGenerator.TestString, ItemsGenerator.TestInt, ItemsGenerator.TestGuid, ItemsGenerator.TestDate);
            Assert.True(result);
        }

        [Fact]
        public async Task TestComplexPathWithBodyParameter()
        {
            var item = await ItemsGenerator.GenerateItem();
            var result = await _service.ComplexPathWithBodyParameter(item);
            Assert.True(result);
        }

        [Fact]
        public async Task TestComplexPathWithNoParameters()
        {
            var result = await _service.ComplexPathWithoutParameters();
            Assert.True(result);
        }

        [Fact]
        public async Task TestComplexPathWithParameters()
        {
            var item = await ItemsGenerator.GenerateItem();
            var result = await _service.ComplexPathWithParameters(item);
            Assert.True(result);
        }

        [Fact]
        public async Task TestComplexPathWithQueryParameters()
        {
            var result = await _service.ComplexPathWithQueryParameters(ItemsGenerator.TestString, ItemsGenerator.TestInt, ItemsGenerator.TestGuid, ItemsGenerator.TestDate);
            Assert.True(result);
        }

        [Fact]
        public async Task TestComplexPathWithUrlParameters()
        {
            var result = await _service.ComplexPathWithUrlParameters(ItemsGenerator.TestString, ItemsGenerator.TestInt, ItemsGenerator.TestGuid, ItemsGenerator.TestDate);
            Assert.True(result);
        }

        [Fact]
        public async Task TestNoPathWithAllParameterTypes()
        {
            var item = await ItemsGenerator.GenerateItem();
            var result = await _service.EmptyPathWithAllParameters(
                item,
                ItemsGenerator.TestString, ItemsGenerator.TestInt, ItemsGenerator.TestGuid, ItemsGenerator.TestDate,
                ItemsGenerator.TestString, ItemsGenerator.TestInt, ItemsGenerator.TestGuid, ItemsGenerator.TestDate);
            Assert.True(result);
        }

        [Fact]
        public async Task TestNoPathWithBodyParameter()
        {
            var item = await ItemsGenerator.GenerateItem();
            var result = await _service.EmptyPathWithBodyParameter(item);
            Assert.True(result);
        }

        [Fact]
        public async Task TestNoPathWithNoParameters()
        {
            var result = await _service.EmptyPathWithoutParameters();
            Assert.True(result);
        }

        [Fact]
        public async Task TestNoPathWithParameters()
        {
            var item = await ItemsGenerator.GenerateItem();
            var result = await _service.EmptyPathWithParameters(item);
            Assert.True(result);
        }

        [Fact]
        public async Task TestNoPathWithQueryParameters()
        {
            var result = await _service.EmptyPathWithQueryParameters(ItemsGenerator.TestString, ItemsGenerator.TestInt, ItemsGenerator.TestGuid, ItemsGenerator.TestDate);
            Assert.True(result);
        }

        [Fact]
        public async Task TestNoPathWithUrlAndQueryParameters()
        {
            var item = await ItemsGenerator.GenerateItem();
            var result = await _service.EmptyPathWithUrlAndQueryParameters(item, ItemsGenerator.TestString, ItemsGenerator.TestInt, ItemsGenerator.TestGuid, ItemsGenerator.TestDate);
            Assert.True(result);
        }

        [Fact]
        public async Task TestNoPathWithUrlParameters()
        {
            var result = await _service.EmptyPathWithUrlParameters(ItemsGenerator.TestString, ItemsGenerator.TestInt, ItemsGenerator.TestGuid, ItemsGenerator.TestDate);
            Assert.True(result);
        }

        [Fact]
        public async Task TestSimplePathWithAllParameterTypes()
        {
            var item = await ItemsGenerator.GenerateItem();
            var result = await _service.SimplePathWithAllParameters(
                item,
                ItemsGenerator.TestString, ItemsGenerator.TestInt, ItemsGenerator.TestGuid, ItemsGenerator.TestDate,
                ItemsGenerator.TestString, ItemsGenerator.TestInt, ItemsGenerator.TestGuid, ItemsGenerator.TestDate);
            Assert.True(result);
        }

        [Fact]
        public async Task TestSimplePathWithBodyParameter()
        {
            var item = await ItemsGenerator.GenerateItem();
            var result = await _service.SimplePathWithBodyParameter(item);
            Assert.True(result);
        }

        [Fact]
        public async Task TestSimplePathWithNoParameters()
        {
            var result = await _service.SimplePathWithoutParameters();
            Assert.True(result);
        }

        [Fact]
        public async Task TestSimplePathWithQueryParameters()
        {
            var result = await _service.SimplePathWithQueryParameters(ItemsGenerator.TestString, ItemsGenerator.TestInt, ItemsGenerator.TestGuid, ItemsGenerator.TestDate);
            Assert.True(result);
        }

        [Fact]
        public async Task TestSimplePathWithUrlParameters()
        {
            var result = await _service.SimplePathWithUrlParameters(ItemsGenerator.TestString, ItemsGenerator.TestInt, ItemsGenerator.TestGuid, ItemsGenerator.TestDate);
            Assert.True(result);
        }
    }
}