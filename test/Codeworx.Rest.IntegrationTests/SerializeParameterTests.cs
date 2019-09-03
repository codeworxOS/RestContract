using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Codeworx.Rest.UnitTests.Api.Contract;
using Codeworx.Rest.UnitTests.Dao;
using Codeworx.Rest.UnitTests.Data;
using Xunit;

namespace Codeworx.Rest.UnitTests
{
    public class SerializeParameterTests: TestServerTestsBase
    {
        private readonly ISerializeParameterController _controller;

        public SerializeParameterTests()
        {
            _controller = new SerializeParameterDao(RestOptions);
        }

        [Fact]
        public async Task TestStringQueryParameter()
        {
            var expectedParameter = ItemsGenerator.TestString;
            var actualParameter = await _controller.GetStringQueryParameter(expectedParameter);
            Assert.Equal(expectedParameter, actualParameter);
        }

        [Fact]
        public async Task TestStringUrlParameter()
        {
            var expectedParameter = ItemsGenerator.TestString;
            var actualParameter = await _controller.GetStringUrlParameter(expectedParameter);
            Assert.Equal(expectedParameter, actualParameter);
        }


        [Fact]
        public async Task TestStringBodyParameter()
        {
            var expectedParameter = ItemsGenerator.TestString;
            var actualParameter = await _controller.GetStringBodyParameter(expectedParameter);
            Assert.Equal(expectedParameter, actualParameter);
        }

        [Fact]
        public async Task TestNullStringQueryParameter()
        {
            var actualParameter = await _controller.GetStringQueryParameter(null);
            Assert.Null(actualParameter);
        }

        [Fact]
        public async Task TestNullStringUrlParameter()
        {
            var actualParameter = await _controller.GetStringUrlParameter(null);
            Assert.Null(actualParameter);
        }


        [Fact]
        public async Task TestNullStringBodyParameter()
        {
            var actualParameter = await _controller.GetStringBodyParameter(null);
            Assert.Null(actualParameter);
        }



        [Fact]
        public async Task TestDateTimeQueryParameter()
        {
            var expectedParameter = ItemsGenerator.TestDate;
            var actualParameter = await _controller.GetDateTimeQueryParameter(expectedParameter);
            Assert.Equal(expectedParameter, actualParameter);
        }

        [Fact]
        public async Task TestDateTimeUrlParameter()
        {
            var expectedParameter = ItemsGenerator.TestDate;
            var actualParameter = await _controller.GetDateTimeUrlParameter(expectedParameter);
            Assert.Equal(expectedParameter, actualParameter);
        }


        [Fact]
        public async Task TestDateTimeBodyParameter()
        {
            var expectedParameter = ItemsGenerator.TestDate;
            var actualParameter = await _controller.GetDateTimeBodyParameter(expectedParameter);
            Assert.Equal(expectedParameter, actualParameter);
        }

        [Fact]
        public async Task TestGuidQueryParameter()
        {
            var expectedParameter = ItemsGenerator.TestGuid;
            var actualParameter = await _controller.GetGuidQueryParameter(expectedParameter);
            Assert.Equal(expectedParameter, actualParameter);
        }

        [Fact]
        public async Task TestGuidUrlParameter()
        {
            var expectedParameter = ItemsGenerator.TestGuid;
            var actualParameter = await _controller.GetGuidUrlParameter(expectedParameter);
            Assert.Equal(expectedParameter, actualParameter);
        }

        [Fact]
        public async Task TestGuidBodyParameter()
        {
            var expectedParameter = ItemsGenerator.TestGuid;
            var actualParameter = await _controller.GetGuidBodyParameter(expectedParameter);
            Assert.Equal(expectedParameter, actualParameter);
        }

        [Fact]
        public async Task TestIntQueryParameter()
        {
            var expectedParameter = ItemsGenerator.TestInt;
            var actualParameter = await _controller.GetIntQueryParameter(expectedParameter);
            Assert.Equal(expectedParameter, actualParameter);
        }

        [Fact]
        public async Task TestIntUrlParameter()
        {
            var expectedParameter = ItemsGenerator.TestInt;
            var actualParameter = await _controller.GetIntUrlParameter(expectedParameter);
            Assert.Equal(expectedParameter, actualParameter);
        }

        [Fact]
        public async Task TestIntBodyParameter()
        {
            var expectedParameter = ItemsGenerator.TestInt;
            var actualParameter = await _controller.GetIntBodyParameter(expectedParameter);
            Assert.Equal(expectedParameter, actualParameter);
        }

        [Fact]
        public async Task TestGuidListQueryParameter()
        {
            var expectedParameter = new List<Guid>
            {
                ItemsGenerator.TestGuid,
                Guid.NewGuid()
            };
            var actualParameter = await _controller.GetGuidListQueryParameter(expectedParameter);
            Assert.Equal(expectedParameter, actualParameter);
        }

        [Fact]
        public async Task TestGuidListUrlParameter()
        {
            var expectedParameter = new List<Guid>
            {
                ItemsGenerator.TestGuid,
                Guid.NewGuid()
            };
            var actualParameter = await _controller.GetGuidListUrlParameter(expectedParameter);
            Assert.Equal(expectedParameter, actualParameter);
        }

        [Fact]
        public async Task TestGuidListBodyParameter()
        {
            var expectedParameter = new List<Guid>
            {
                ItemsGenerator.TestGuid,
                Guid.NewGuid()
            };
            var actualParameter = await _controller.GetGuidListBodyParameter(expectedParameter);
            Assert.Equal(expectedParameter, actualParameter);
        }

        [Fact]
        public async Task TestItemBodyParameter()
        {
            var expectedItem = await ItemsGenerator.GenerateItem();
            var actualItem = await _controller.GetItemBodyParameter(expectedItem);
            Assert.Equal(expectedItem, actualItem);
        }


        [Fact]
        public async Task TestNullItemBodyParameter()
        {
            var actualItem = await _controller.GetItemBodyParameter(null);
            Assert.Null(actualItem);
        }


        [Fact]
        public async Task TestBodyParameterWithNoAttribute()
        {
            var expectedItem = await ItemsGenerator.GenerateItem();
            var actualItem = await _controller.GetItemBodyParameterWithNoAttribute(expectedItem);
            Assert.Equal(expectedItem, actualItem);
        }

        [Fact]
        public async Task TestBodyParameterAsLastParameter()
        {
            var expectedItem = await ItemsGenerator.GenerateItem();
            var actualItem = await _controller.GetItemBodyParameterAsLastParameter(ItemsGenerator.TestGuid, "Test", expectedItem);
            Assert.Equal(expectedItem, actualItem);
        }
    }
}
