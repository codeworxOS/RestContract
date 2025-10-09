using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Codeworx.Rest.UnitTests.Api.Contract;
using Codeworx.Rest.UnitTests.Data;
using Codeworx.Rest.UnitTests.Model;
using Xunit;

namespace Codeworx.Rest.UnitTests
{
    public class SerializeParameterTests : TestServerTestsBase
    {
        public static IEnumerable<object[]> DateOffsetParameters = new List<object[]>
        {
            new object[] {ItemsGenerator.TestDateOffset, FormatterSelection.Json},
            new object[] {null, FormatterSelection.Json},
            new object[] {ItemsGenerator.TestDateOffset, FormatterSelection.Protobuf},
            new object[] {null, FormatterSelection.Protobuf},
            new object[] {ItemsGenerator.TestDateOffset, FormatterSelection.NewtonsoftJson},
            new object[] {null, FormatterSelection.NewtonsoftJson},
        };

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

        public static IEnumerable<object[]> GuidListParameters;

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
            new object[] {ItemsGenerator.TestNegativeInt, FormatterSelection.Json},
            new object[] {null, FormatterSelection.Json},
            new object[] {ItemsGenerator.TestInt, FormatterSelection.Protobuf},
            new object[] {ItemsGenerator.TestNegativeInt, FormatterSelection.Protobuf},
            new object[] {null, FormatterSelection.Protobuf},
            new object[] {ItemsGenerator.TestInt, FormatterSelection.NewtonsoftJson},
            new object[] {ItemsGenerator.TestNegativeInt, FormatterSelection.NewtonsoftJson},
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
            new object[] {ItemsGenerator.TestStringForEscapeWithSlashes, FormatterSelection.Json},
            new object[] {ItemsGenerator.TestStringForEscapeWithSlashes, FormatterSelection.Protobuf},
            new object[] {ItemsGenerator.TestStringForEscapeWithSlashes, FormatterSelection.NewtonsoftJson},
            new object[] {ItemsGenerator.TestStringSpeciaChars, FormatterSelection.Json},
            new object[] {ItemsGenerator.TestStringSpeciaChars, FormatterSelection.Protobuf},
            new object[] {ItemsGenerator.TestStringSpeciaChars, FormatterSelection.NewtonsoftJson},
        };

        static SerializeParameterTests()
        {
            var guidList = new Guid[]
            {
                ItemsGenerator.TestGuid,
                Guid.NewGuid()
            };

            GuidListParameters = new List<object[]>
            {
                new object[] { guidList, FormatterSelection.Json},
                new object[] { null, FormatterSelection.Json },
                new object[] { guidList, FormatterSelection.Protobuf},
                new object[] { null, FormatterSelection.Protobuf },
                new object[] { guidList, FormatterSelection.NewtonsoftJson},
                new object[] { null, FormatterSelection.NewtonsoftJson },
            };
        }


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
        [MemberData(nameof(DateOffsetParameters))]
        public async Task TestDateTimeOffsetBodyParameter(DateTimeOffset? expectedParameter, FormatterSelection formatter)
        {
            var client = Client<ISerializeParameterController>(formatter);
            var actualParameter = await client.GetDateTimeOffsetBodyParameter(expectedParameter);
            Assert.Equal(expectedParameter, actualParameter);

            if (expectedParameter.HasValue)
            {
                Assert.Equal(expectedParameter.Value.Offset, actualParameter.Value.Offset);
                Assert.Equal(expectedParameter.Value.UtcDateTime, actualParameter.Value.UtcDateTime);
            }
        }

        [Theory]
        [MemberData(nameof(DateOffsetParameters))]
        public async Task TestDateTimeOffsetQueryParameter(DateTimeOffset? expectedParameter, FormatterSelection formatter)
        {
            var client = Client<ISerializeParameterController>(formatter);
            var actualParameter = await client.GetDateTimeOffsetQueryParameter(expectedParameter);
            Assert.Equal(expectedParameter, actualParameter);

            if (expectedParameter.HasValue)
            {
                Assert.Equal(expectedParameter.Value.Offset, actualParameter.Value.Offset);
                Assert.Equal(expectedParameter.Value.UtcDateTime, actualParameter.Value.UtcDateTime);
            }
        }

        [Theory]
        [MemberData(nameof(DateOffsetParameters))]
        public async Task TestDateTimeOffsetQueryExplicitParameter(DateTimeOffset? expectedParameter, FormatterSelection formatter)
        {
            var client = Client<ISerializeParameterController>(formatter);
            var actualParameter = await client.GetDateTimeOffsetQueryExplicitParameter(expectedParameter);
            Assert.Equal(expectedParameter, actualParameter);

            if (expectedParameter.HasValue)
            {
                Assert.Equal(expectedParameter.Value.Offset, actualParameter.Value.Offset);
                Assert.Equal(expectedParameter.Value.UtcDateTime, actualParameter.Value.UtcDateTime);
            }
        }

        [Theory]
        [MemberData(nameof(DateOffsetParameters))]
        public async Task TestDateTimeOffsetUrlParameter(DateTimeOffset? expectedParameter, FormatterSelection formatter)
        {
            var client = Client<ISerializeParameterController>(formatter);
            var actualParameter = await client.GetDateTimeOffsetUrlParameter(expectedParameter);
            Assert.Equal(expectedParameter, actualParameter);

            if (expectedParameter.HasValue)
            {
                Assert.Equal(expectedParameter.Value.Offset, actualParameter.Value.Offset);
                Assert.Equal(expectedParameter.Value.UtcDateTime, actualParameter.Value.UtcDateTime);
            }
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
        public async Task TestDateTimeQueryExplicitParameter(DateTime? expectedParameter, FormatterSelection formatter)
        {
            var client = Client<ISerializeParameterController>(formatter);
            var actualParameter = await client.GetDateTimeQueryExplicitParameter(expectedParameter);
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
        public async Task TestDecimalQueryExplicitParameter(decimal? expectedParameter, FormatterSelection formatter)
        {
            var client = Client<ISerializeParameterController>(formatter);
            var actualParameter = await client.GetDecimalQueryExplicitParameter(expectedParameter);
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
        public async Task TestDoubleQueryExplicitParameter(double? expectedParameter, FormatterSelection formatter)
        {
            var client = Client<ISerializeParameterController>(formatter);
            var actualParameter = await client.GetDoubleQueryExplicitParameter(expectedParameter);
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
        public async Task TestFloatQueryExplicitParameter(float? expectedParameter, FormatterSelection formatter)
        {
            var client = Client<ISerializeParameterController>(formatter);
            var actualParameter = await client.GetFloatQueryExplicitParameter(expectedParameter);
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
        public async Task TestGuidListBodyParameter(Guid[] expectedParameter, FormatterSelection formatter)
        {
            var client = Client<ISerializeParameterController>(formatter);
            var actualParameter = await client.GetGuidListBodyParameter(5, expectedParameter?.ToList());
            Assert.Equal(expectedParameter, actualParameter?.ToArray());
        }

        [Theory()]
        [MemberData(nameof(GuidListParameters))]
        public async Task TestGuidListQueryParameter(Guid[] expectedParameter, FormatterSelection formatter)
        {
            var client = Client<ISerializeParameterController>(formatter);
            var actualParameter = await client.GetGuidListQueryParameter(expectedParameter?.ToList());
            expectedParameter = expectedParameter ?? new Guid[] { };
            Assert.Equal(expectedParameter, actualParameter.ToArray());
        }

        [Theory()]
        [MemberData(nameof(GuidListParameters))]
        public async Task TestGuidListQueryExplicitParameter(Guid[] expectedParameter, FormatterSelection formatter)
        {
            var client = Client<ISerializeParameterController>(formatter);
            var actualParameter = await client.GetGuidListQueryExplicitParameter(expectedParameter?.ToList());
            expectedParameter = expectedParameter ?? new Guid[] { };
            Assert.Equal(expectedParameter, actualParameter.ToArray());
        }

        [Theory(Skip = "might not be possible in asp.net core as well...")]
        [MemberData(nameof(GuidListParameters))]
        public async Task TestGuidListUrlParameter(Guid[] expectedParameter, FormatterSelection formatter)
        {
            var client = Client<ISerializeParameterController>(formatter);
            var actualParameter = await client.GetGuidListUrlParameter(expectedParameter?.ToList());
            expectedParameter = expectedParameter ?? new Guid[] { };
            Assert.Equal(expectedParameter, actualParameter.ToArray());
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
        public async Task TestGuidQueryExplicitParameter(Guid? expectedParameter, FormatterSelection formatter)
        {
            var client = Client<ISerializeParameterController>(formatter);
            var actualParameter = await client.GetGuidQueryExplicitParameter(expectedParameter);
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
        public async Task TestIntQueryExplicitParameter(int? expectedParameter, FormatterSelection formatter)
        {
            var client = Client<ISerializeParameterController>(formatter);
            var actualParameter = await client.GetIntQueryExplicitParameter(expectedParameter);
            Assert.Equal(expectedParameter, actualParameter);
        }

        [Theory]
        [MemberData(nameof(IntParameters))]
        public async Task TestIntQueryParameterInCultureSV(int? expectedParameter, FormatterSelection formatter)
        {
            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("sv");
            var client = Client<ISerializeParameterController>(formatter);
            var actualParameter = await client.GetIntQueryParameter(expectedParameter);
            Assert.Equal(expectedParameter, actualParameter);
        }

        [Theory]
        [MemberData(nameof(IntParameters))]
        public async Task TestIntQueryExplicitParameterInCultureSV(int? expectedParameter, FormatterSelection formatter)
        {
            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("sv");
            var client = Client<ISerializeParameterController>(formatter);
            var actualParameter = await client.GetIntQueryExplicitParameter(expectedParameter);
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
        public async Task TestStringQueryExplicitParameter(string expectedParameter, FormatterSelection formatter)
        {
            var client = Client<ISerializeParameterController>(formatter);
            var actualParameter = await client.GetStringQueryExplicitParameter(expectedParameter);
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