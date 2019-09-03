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

        public static  IEnumerable<object[]> StringParameters = new List<object[]>
        {
            new object[] {ItemsGenerator.TestString},
            new object[] {null},
        };

        public static IEnumerable<object[]> DateParameters = new List<object[]>
        {
            new object[] {ItemsGenerator.TestDate},
            new object[] {null},
        };

        public static IEnumerable<object[]> GuidParameters = new List<object[]>
        {
            new object[] {ItemsGenerator.TestGuid},
            new object[] {null},
        };

        public static IEnumerable<object[]> IntParameters = new List<object[]>
        {
            new object[] {ItemsGenerator.TestInt},
            new object[] {null},
        };

        private static List<Guid> _guidList = new List<Guid>
        {
            ItemsGenerator.TestGuid,
            Guid.NewGuid()
        };

        public static IEnumerable<object[]> GuidListParameters = new List<object[]>
        {
            new object[] {_guidList},
            new object[] {null},
        };

        [Theory]
        [MemberData(nameof(StringParameters))]
        public async Task TestStringQueryParameter(string expectedParameter)
        {
            var actualParameter = await _controller.GetStringQueryParameter(expectedParameter);
            Assert.Equal(expectedParameter, actualParameter);
        }

        [Theory]
        [MemberData(nameof(StringParameters))]
        public async Task TestStringUrlParameter(string expectedParameter)
        {
            var actualParameter = await _controller.GetStringUrlParameter(expectedParameter);
            Assert.Equal(expectedParameter, actualParameter);
        }

        [Theory]
        [MemberData(nameof(StringParameters))]
        public async Task TestStringBodyParameter(string expectedParameter)
        {
            var actualParameter = await _controller.GetStringBodyParameter(expectedParameter);
            Assert.Equal(expectedParameter, actualParameter);
        }

        [Theory]
        [MemberData(nameof(DateParameters))]
        public async Task TestDateTimeQueryParameter(DateTime? expectedParameter)
        {
            var actualParameter = await _controller.GetDateTimeQueryParameter(expectedParameter);
            Assert.Equal(expectedParameter, actualParameter);
        }

        [Theory]
        [MemberData(nameof(DateParameters))]
        public async Task TestDateTimeUrlParameter(DateTime? expectedParameter)
        {
            var actualParameter = await _controller.GetDateTimeUrlParameter(expectedParameter);
            Assert.Equal(expectedParameter, actualParameter);
        }

        [Theory]
        [MemberData(nameof(DateParameters))]
        public async Task TestDateTimeBodyParameter(DateTime? expectedParameter)
        {
            var actualParameter = await _controller.GetDateTimeBodyParameter(expectedParameter);
            Assert.Equal(expectedParameter, actualParameter);
        }

        [Theory]
        [MemberData(nameof(GuidParameters))]
        public async Task TestGuidQueryParameter(Guid? expectedParameter)
        {
            var actualParameter = await _controller.GetGuidQueryParameter(expectedParameter);
            Assert.Equal(expectedParameter, actualParameter);
        }

        [Theory]
        [MemberData(nameof(GuidParameters))]
        public async Task TestGuidUrlParameter(Guid? expectedParameter)
        {
            var actualParameter = await _controller.GetGuidUrlParameter(expectedParameter);
            Assert.Equal(expectedParameter, actualParameter);
        }

        [Theory]
        [MemberData(nameof(GuidParameters))]
        public async Task TestGuidBodyParameter(Guid? expectedParameter)
        {
            var actualParameter = await _controller.GetGuidBodyParameter(expectedParameter);
            Assert.Equal(expectedParameter, actualParameter);
        }

        [Theory]
        [MemberData(nameof(IntParameters))]
        public async Task TestIntQueryParameter(int? expectedParameter)
        {
            var actualParameter = await _controller.GetIntQueryParameter(expectedParameter);
            Assert.Equal(expectedParameter, actualParameter);
        }

        [Theory]
        [MemberData(nameof(IntParameters))]
        public async Task TestIntUrlParameter(int? expectedParameter)
        {
            var actualParameter = await _controller.GetIntUrlParameter(expectedParameter);
            Assert.Equal(expectedParameter, actualParameter);
        }

        [Theory]
        [MemberData(nameof(IntParameters))]
        public async Task TestIntBodyParameter(int? expectedParameter)
        {
            var actualParameter = await _controller.GetIntBodyParameter(expectedParameter);
            Assert.Equal(expectedParameter, actualParameter);
        }

        [Theory]
        [MemberData(nameof(GuidListParameters))]
        public async Task TestGuidListQueryParameter(List<Guid> expectedParameter)
        {
            var actualParameter = await _controller.GetGuidListQueryParameter(expectedParameter);
            Assert.Equal(expectedParameter, actualParameter);
        }

        [Theory]
        [MemberData(nameof(GuidListParameters))]
        public async Task TestGuidListUrlParameter(List<Guid> expectedParameter)
        {
            var actualParameter = await _controller.GetGuidListUrlParameter(expectedParameter);
            Assert.Equal(expectedParameter, actualParameter);
        }

        [Theory]
        [MemberData(nameof(GuidListParameters))]
        public async Task TestGuidListBodyParameter(List<Guid> expectedParameter)
        {
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
            Assert.NotEqual(expectedItem, actualItem);
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
