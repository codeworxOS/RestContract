using System.Threading.Tasks;
using Codeworx.Rest.UnitTests.Api.Contract;
using Codeworx.Rest.UnitTests.Dao;
using Codeworx.Rest.UnitTests.Data;
using Xunit;

namespace Codeworx.Rest.UnitTests
{
    public class SerializeResultTests: TestServerTestsBase
    {
        private readonly ISerializeResultController _controller;

        public SerializeResultTests()
        {
            _controller = new SerializeResultDao(RestOptions);
        }

        [Fact]
        public async Task TestNoResult()
        {
            await _controller.NoResult();
        }

        [Fact]
        public async Task TestStringResult()
        {
            var result = await _controller.StringResult();
            Assert.Equal(ItemsGenerator.TestString, result);
        }

        [Fact]
        public async Task TestDateTimeResult()
        {
            var result = await _controller.DateTimeResult();
            Assert.Equal(ItemsGenerator.TestDate, result);
        }

        [Fact]
        public async Task TestNullableDateTimeResult()
        {
            var result = await _controller.NullableDateTimeResult();
            Assert.Equal(ItemsGenerator.TestDate, result);
        }

        [Fact]
        public async Task TestGuidResult()
        {
            var result = await _controller.GuidResult();
            Assert.Equal(ItemsGenerator.TestGuid, result);
        }

        [Fact]
        public async Task TestNullableGuidResult()
        {
            var result = await _controller.NullableGuidResult();
            Assert.Equal(ItemsGenerator.TestGuid, result);
        }

        [Fact]
        public async Task TestItemResult()
        {
            var result = await _controller.ItemResult();

            var expectedItem = await ItemsGenerator.GenerateItem();
            Assert.Equal(expectedItem, result);
        }

        [Fact]
        public async Task TestItemNullResult()
        {
            var result = await _controller.ItemNullResult();
            Assert.Null(result);
        }

        [Fact]
        public async Task TestStringListResult()
        {
            var result = await _controller.StringListResult();
            Assert.Contains(ItemsGenerator.TestString, result);
        }

        [Fact]
        public async Task TestDateTimeListResult()
        {
            var result = await _controller.DateTimeListResult();
            Assert.Contains(ItemsGenerator.TestDate, result);
        }

        [Fact]
        public async Task TestGuidListResult()
        {
            var result = await _controller.GuidListResult();
            Assert.Contains(ItemsGenerator.TestGuid, result);

        }

        [Fact]
        public async Task TestItemListResult()
        {
            var result = await _controller.ItemListResult();

            var expectedItems = await ItemsGenerator.GenerateItems();
            Assert.Equal(expectedItems, result);
        }
    }
}
