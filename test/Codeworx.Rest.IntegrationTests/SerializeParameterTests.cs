using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Codeworx.Rest.UnitTests.Api.Contract;
using Codeworx.Rest.UnitTests.Data;
using Codeworx.Rest.UnitTests.Model;
using Xunit;

namespace Codeworx.Rest.UnitTests
{
    public class SerializeParameterTests : TestServerTestsBase
    {
        public static IEnumerable<object[]> DateParameters = new List<object[]>
        {
            new object[] {ItemsGenerator.TestDate, FormatterSelection.Json},
            new object[] {null, FormatterSelection.Json},
            new object[] {ItemsGenerator.TestDate, FormatterSelection.Protobuf},
            new object[] {null, FormatterSelection.Protobuf},
            new object[] {ItemsGenerator.TestDate, FormatterSelection.NewtonsoftJson},
            new object[] {null, FormatterSelection.NewtonsoftJson},
        };

        public static IEnumerable<object[]> DecimalParameters = new List<object[]>
        {
            new object[] {ItemsGenerator.TestDecimal, FormatterSelection.Json },
            new object[] { null, FormatterSelection.Json },
            new object[] {ItemsGenerator.TestDecimal, FormatterSelection.Protobuf },
            new object[] { null, FormatterSelection.Protobuf },
            new object[] {ItemsGenerator.TestDecimal, FormatterSelection.NewtonsoftJson },
            new object[] { null, FormatterSelection.NewtonsoftJson },
        };

        public static IEnumerable<object[]> DoubleParameters = new List<object[]>
        {
            new object[] {ItemsGenerator.TestDouble, FormatterSelection.Json},
            new object[] { null, FormatterSelection.Json },
            new object[] {ItemsGenerator.TestDouble, FormatterSelection.Protobuf},
            new object[] { null, FormatterSelection.Protobuf },
            new object[] {ItemsGenerator.TestDouble, FormatterSelection.NewtonsoftJson},
            new object[] { null, FormatterSelection.NewtonsoftJson }
        };

        public static IEnumerable<object[]> FloatParameters = new List<object[]>
        {
            new object[] {ItemsGenerator.TestFloat, FormatterSelection.Json},
            new object[] { null, FormatterSelection.Json },
            new object[] {ItemsGenerator.TestFloat, FormatterSelection.Protobuf},
            new object[] { null, FormatterSelection.Protobuf },
            new object[] {ItemsGenerator.TestFloat, FormatterSelection.NewtonsoftJson},
            new object[] { null, FormatterSelection.NewtonsoftJson }
        };

        public static IEnumerable<object[]> FormatterParameters = new List<object[]>
        {
           new object[] {FormatterSelection.Json },
           new object[] {FormatterSelection.Protobuf },
           new object[] {FormatterSelection.NewtonsoftJson },
        };

        public static IEnumerable<object[]> GuidListParameters = new List<object[]>
        {
            new object[] {_guidList, FormatterSelection.Json},
            new object[] { null, FormatterSelection.Json },
            new object[] {_guidList, FormatterSelection.Protobuf},
            new object[] { null, FormatterSelection.Protobuf },
            new object[] {_guidList, FormatterSelection.NewtonsoftJson},
            new object[] { null, FormatterSelection.NewtonsoftJson },
        };

        public static IEnumerable<object[]> GuidParameters = new List<object[]>
        {
            new object[] {ItemsGenerator.TestGuid, FormatterSelection.Json},
            new object[] {null, FormatterSelection.Json},
            new object[] {ItemsGenerator.TestGuid, FormatterSelection.Protobuf},
            new object[] {null, FormatterSelection.Protobuf},
            new object[] {ItemsGenerator.TestGuid, FormatterSelection.NewtonsoftJson},
            new object[] {null, FormatterSelection.NewtonsoftJson},
        };

        public static IEnumerable<object[]> IntParameters = new List<object[]>
        {
            new object[] {ItemsGenerator.TestInt, FormatterSelection.Json},
            new object[] {null, FormatterSelection.Json},
            new object[] {ItemsGenerator.TestInt, FormatterSelection.Protobuf},
            new object[] {null, FormatterSelection.Protobuf},
            new object[] {ItemsGenerator.TestInt, FormatterSelection.NewtonsoftJson},
            new object[] {null, FormatterSelection.NewtonsoftJson},
        };

        public static IEnumerable<object[]> StringParameters = new List<object[]>
        {
            new object[] {ItemsGenerator.TestString, FormatterSelection.Json},
            new object[] {null, FormatterSelection.Json},
            new object[] {ItemsGenerator.TestString, FormatterSelection.Protobuf},
            new object[] {null, FormatterSelection.Protobuf},
            new object[] {ItemsGenerator.TestString, FormatterSelection.NewtonsoftJson},
            new object[] {null, FormatterSelection.NewtonsoftJson},
            new object[] {ItemsGenerator.TestStringForEscape, FormatterSelection.Json},
            new object[] {ItemsGenerator.TestStringForEscape, FormatterSelection.Protobuf},
            new object[] {ItemsGenerator.TestStringForEscape, FormatterSelection.NewtonsoftJson},
            new object[] {ItemsGenerator.TestStringSpeciaChars, FormatterSelection.Json},
            new object[] {ItemsGenerator.TestStringSpeciaChars, FormatterSelection.Protobuf},
            new object[] {ItemsGenerator.TestStringSpeciaChars, FormatterSelection.NewtonsoftJson},
        };

        private static List<Guid> _guidList = new List<Guid>
        {
            ItemsGenerator.TestGuid,
            Guid.NewGuid()
        };

        [Theory]
        [MemberData(nameof(FormatterParameters))]
        public async Task TestBodyParameterAsLastParameter(FormatterSelection formatter)
        {
            var client = Client<ISerializeParameterController>(formatter);
            var expectedItem = await ItemsGenerator.GenerateItem();
            var actualItem = await client.GetItemBodyParameterAsLastParameter(ItemsGenerator.TestGuid, "Test", expectedItem);
            Assert.Equal(expectedItem, actualItem);
        }

        [Theory]
        [MemberData(nameof(FormatterParameters))]
        public async Task TestBodyParameterWithNoAttribute(FormatterSelection formatter)
        {
            var client = Client<ISerializeParameterController>(formatter);
            var expectedItem = await ItemsGenerator.GenerateItem();
            var actualItem = await client.GetItemBodyParameterWithNoAttribute(expectedItem);
            Assert.NotEqual(expectedItem, actualItem);
        }

        [Theory]
        [MemberData(nameof(DateParameters))]
        public async Task TestDateTimeBodyParameter(DateTime? expectedParameter, FormatterSelection formatter)
        {
            var client = Client<ISerializeParameterController>(formatter);
            var actualParameter = await client.GetDateTimeBodyParameter(expectedParameter);
            Assert.Equal(expectedParameter, actualParameter);
        }

        [Theory]
        [MemberData(nameof(DateParameters))]
        public async Task TestDateTimeQueryParameter(DateTime? expectedParameter, FormatterSelection formatter)
        {
            var client = Client<ISerializeParameterController>(formatter);
            var actualParameter = await client.GetDateTimeQueryParameter(expectedParameter);
            Assert.Equal(expectedParameter, actualParameter);
        }

        [Theory]
        [MemberData(nameof(DateParameters))]
        public async Task TestDateTimeUrlParameter(DateTime? expectedParameter, FormatterSelection formatter)
        {
            var client = Client<ISerializeParameterController>(formatter);
            var actualParameter = await client.GetDateTimeUrlParameter(expectedParameter);
            Assert.Equal(expectedParameter, actualParameter);
        }

        [Theory]
        [MemberData(nameof(DecimalParameters))]
        public async Task TestDecimalBodyParameter(decimal? expectedParameter, FormatterSelection formatter)
        {
            var client = Client<ISerializeParameterController>(formatter);
            var actualParameter = await client.GetDecimalBodyParameter(expectedParameter);
            Assert.Equal(expectedParameter, actualParameter);
        }

        [Theory]
        [MemberData(nameof(DecimalParameters))]
        public async Task TestDecimalQueryParameter(decimal? expectedParameter, FormatterSelection formatter)
        {
            var client = Client<ISerializeParameterController>(formatter);
            var actualParameter = await client.GetDecimalQueryParameter(expectedParameter);
            Assert.Equal(expectedParameter, actualParameter);
        }

        [Theory]
        [MemberData(nameof(DecimalParameters))]
        public async Task TestDecimalUrlParameter(decimal? expectedParameter, FormatterSelection formatter)
        {
            var client = Client<ISerializeParameterController>(formatter);
            var actualParameter = await client.GetDecimalUrlParameter(expectedParameter);
            Assert.Equal(expectedParameter, actualParameter);
        }

        [Theory]
        [MemberData(nameof(DoubleParameters))]
        public async Task TestDoubleBodyParameter(double? expectedParameter, FormatterSelection formatter)
        {
            var client = Client<ISerializeParameterController>(formatter);
            var actualParameter = await client.GetDoubleBodyParameter(expectedParameter);
            Assert.Equal(expectedParameter, actualParameter);
        }

        [Theory]
        [MemberData(nameof(DoubleParameters))]
        public async Task TestDoubleQueryParameter(double? expectedParameter, FormatterSelection formatter)
        {
            var client = Client<ISerializeParameterController>(formatter);
            var actualParameter = await client.GetDoubleQueryParameter(expectedParameter);
            Assert.Equal(expectedParameter, actualParameter);
        }

        [Theory]
        [MemberData(nameof(DoubleParameters))]
        public async Task TestDoubleUrlParameter(double? expectedParameter, FormatterSelection formatter)
        {
            var client = Client<ISerializeParameterController>(formatter);
            var actualParameter = await client.GetDoubleUrlParameter(expectedParameter);
            Assert.Equal(expectedParameter, actualParameter);
        }

        [Theory]
        [MemberData(nameof(FloatParameters))]
        public async Task TestFloatBodyParameter(float? expectedParameter, FormatterSelection formatter)
        {
            var client = Client<ISerializeParameterController>(formatter);
            var actualParameter = await client.GetFloatBodyParameter(expectedParameter);
            Assert.Equal(expectedParameter, actualParameter);
        }

        [Theory]
        [MemberData(nameof(FloatParameters))]
        public async Task TestFloatQueryParameter(float? expectedParameter, FormatterSelection formatter)
        {
            var client = Client<ISerializeParameterController>(formatter);
            var actualParameter = await client.GetFloatQueryParameter(expectedParameter);
            Assert.Equal(expectedParameter, actualParameter);
        }

        [Theory]
        [MemberData(nameof(FloatParameters))]
        public async Task TestFloatUrlParameter(float? expectedParameter, FormatterSelection formatter)
        {
            var client = Client<ISerializeParameterController>(formatter);
            var actualParameter = await client.GetFloatUrlParameter(expectedParameter);
            Assert.Equal(expectedParameter, actualParameter);
        }

        [Theory]
        [MemberData(nameof(GuidParameters))]
        public async Task TestGuidBodyParameter(Guid? expectedParameter, FormatterSelection formatter)
        {
            var client = Client<ISerializeParameterController>(formatter);
            var actualParameter = await client.GetGuidBodyParameter(expectedParameter);
            Assert.Equal(expectedParameter, actualParameter);
        }

        [Theory]
        [MemberData(nameof(GuidListParameters))]
        public async Task TestGuidListBodyParameter(List<Guid> expectedParameter, FormatterSelection formatter)
        {
            var client = Client<ISerializeParameterController>(formatter);
            var actualParameter = await client.GetGuidListBodyParameter(5, expectedParameter);
            Assert.Equal(expectedParameter, actualParameter);
        }

        [Theory(Skip = "Future feature -> custom model binders")]
        [MemberData(nameof(GuidListParameters))]
        public async Task TestGuidListQueryParameter(List<Guid> expectedParameter, FormatterSelection formatter)
        {
            var client = Client<ISerializeParameterController>(formatter);
            var actualParameter = await client.GetGuidListQueryParameter(expectedParameter);
            Assert.Equal(expectedParameter, actualParameter);
        }

        [Theory(Skip = "Future feature -> custom model binders")]
        [MemberData(nameof(GuidListParameters))]
        public async Task TestGuidListUrlParameter(List<Guid> expectedParameter, FormatterSelection formatter)
        {
            var client = Client<ISerializeParameterController>(formatter);
            var actualParameter = await client.GetGuidListUrlParameter(expectedParameter);
            Assert.Equal(expectedParameter, actualParameter);
        }

        [Theory]
        [MemberData(nameof(GuidParameters))]
        public async Task TestGuidQueryParameter(Guid? expectedParameter, FormatterSelection formatter)
        {
            var client = Client<ISerializeParameterController>(formatter);
            var actualParameter = await client.GetGuidQueryParameter(expectedParameter);
            Assert.Equal(expectedParameter, actualParameter);
        }

        [Theory]
        [MemberData(nameof(GuidParameters))]
        public async Task TestGuidUrlParameter(Guid? expectedParameter, FormatterSelection formatter)
        {
            var client = Client<ISerializeParameterController>(formatter);
            var actualParameter = await client.GetGuidUrlParameter(expectedParameter);
            Assert.Equal(expectedParameter, actualParameter);
        }

        [Theory]
        [MemberData(nameof(IntParameters))]
        public async Task TestIntBodyParameter(int? expectedParameter, FormatterSelection formatter)
        {
            var client = Client<ISerializeParameterController>(formatter);
            var actualParameter = await client.GetIntBodyParameter(expectedParameter);
            Assert.Equal(expectedParameter, actualParameter);
        }

        [Theory]
        [MemberData(nameof(IntParameters))]
        public async Task TestIntQueryParameter(int? expectedParameter, FormatterSelection formatter)
        {
            var client = Client<ISerializeParameterController>(formatter);
            var actualParameter = await client.GetIntQueryParameter(expectedParameter);
            Assert.Equal(expectedParameter, actualParameter);
        }

        [Theory]
        [MemberData(nameof(IntParameters))]
        public async Task TestIntUrlParameter(int? expectedParameter, FormatterSelection formatter)
        {
            var client = Client<ISerializeParameterController>(formatter);
            var actualParameter = await client.GetIntUrlParameter(expectedParameter);
            Assert.Equal(expectedParameter, actualParameter);
        }

        [Theory]
        [MemberData(nameof(FormatterParameters))]
        public async Task TestItemBodyParameter(FormatterSelection formatter)
        {
            var client = Client<ISerializeParameterController>(formatter);
            var expectedItem = await ItemsGenerator.GenerateItem();
            var actualItem = await client.GetItemBodyParameter(expectedItem);
            Assert.Equal(expectedItem, actualItem);
        }

        [Theory]
        [MemberData(nameof(FormatterParameters))]
        public async Task TestEmptyItemBodyParameter(FormatterSelection formatter)
        {
            var client = Client<ISerializeParameterController>(formatter);
            var expectedItem = new Item();
            var actualItem = await client.GetItemBodyParameter(expectedItem);
            Assert.Equal(expectedItem, actualItem);
        }

        [Theory]
        [MemberData(nameof(FormatterParameters))]
        public async Task TestEmptyGuidListBodyParameter(FormatterSelection formatter)
        {
            var client = Client<ISerializeParameterController>(formatter);
            var expectedItem = new List<Guid>();
            var actualItem = await client.GetGuidListBodyParameter(5, expectedItem);
            Assert.Equal(expectedItem, actualItem);
        }

        [Theory]
        [MemberData(nameof(FormatterParameters))]
        public async Task TestNullItemBodyParameter(FormatterSelection formatter)
        {
            var client = Client<ISerializeParameterController>(formatter);
            var actualItem = await client.GetItemBodyParameter(null);
            Assert.Null(actualItem);
        }

        [Theory]
        [MemberData(nameof(StringParameters))]
        public async Task TestStringBodyParameter(string expectedParameter, FormatterSelection formatter)
        {
            var client = Client<ISerializeParameterController>(formatter);
            var actualParameter = await client.GetStringBodyParameter(expectedParameter);
            Assert.Equal(expectedParameter, actualParameter);
        }

        [Theory]
        [MemberData(nameof(StringParameters))]
        public async Task TestStringQueryParameter(string expectedParameter, FormatterSelection formatter)
        {
            var client = Client<ISerializeParameterController>(formatter);
            var actualParameter = await client.GetStringQueryParameter(expectedParameter);
            Assert.Equal(expectedParameter, actualParameter);
        }

        [Theory]
        [MemberData(nameof(StringParameters))]
        public async Task TestStringUrlParameter(string expectedParameter, FormatterSelection formatter)
        {
            var client = Client<ISerializeParameterController>(formatter);
            var actualParameter = await client.GetStringUrlParameter(expectedParameter);
            Assert.Equal(expectedParameter, actualParameter);
        }
    }
}