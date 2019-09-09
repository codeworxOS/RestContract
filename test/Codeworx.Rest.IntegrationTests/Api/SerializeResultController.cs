using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Codeworx.Rest.UnitTests.Api.Contract;
using Codeworx.Rest.UnitTests.Data;
using Codeworx.Rest.UnitTests.Model;

namespace Codeworx.Rest.UnitTests.Api
{
    public class SerializeResultController : ISerializeResultController
    {
        public async Task NoResult()
        {
            await Task.CompletedTask;
        }

        public async Task<string> StringResult()
        {
            return await Task.FromResult(ItemsGenerator.TestString);
        }

        public async Task<DateTime> DateTimeResult()
        {
            return await Task.FromResult(ItemsGenerator.TestDate);
        }

        public async Task<DateTime?> NullableDateTimeResult()
        {
            return await Task.FromResult(ItemsGenerator.TestDate);
        }

        public async Task<Guid> GuidResult()
        {
            return await Task.FromResult(ItemsGenerator.TestGuid);
        }

        public async Task<Guid?> NullableGuidResult()
        {
            return await Task.FromResult(ItemsGenerator.TestGuid);
        }

        public async Task<int> IntResult()
        {
            return await Task.FromResult(ItemsGenerator.TestInt);
        }

        public async Task<int?> NullableIntResult()
        {
            return await Task.FromResult(ItemsGenerator.TestInt);
        }

        public async Task<decimal> DecimalResult()
        {
            return await Task.FromResult(ItemsGenerator.TestDecimal);
        }

        public async Task<decimal?> NullableDecimalResult()
        {
            return await Task.FromResult(ItemsGenerator.TestDecimal);
        }

        public async Task<double> DoubleResult()
        {
            return await Task.FromResult(ItemsGenerator.TestDouble);
        }

        public async Task<double?> NullableDoubleResult()
        {
            return await Task.FromResult(ItemsGenerator.TestDouble);
        }

        public async Task<float> FloatResult()
        {
            return await Task.FromResult(ItemsGenerator.TestFloat);
        }

        public async Task<float?> NullableFloatResult()
        {
            return await Task.FromResult(ItemsGenerator.TestFloat);
        }

        public async Task<Item> ItemResult()
        {
            var item = await ItemsGenerator.GenerateItem();
            return item;
        }

        public async Task<Item> ItemNullResult()
        {
            return await Task.FromResult<Item>(null);
        }

        public async Task<List<string>> StringListResult()
        {
            var resultList = new List<string>
            {
                ItemsGenerator.TestString,
                "Test 1",
                "Test 2"
            };
            return await Task.FromResult(resultList);
        }

        public async Task<List<DateTime>> DateTimeListResult()
        {
            var resultList = new List<DateTime>
            {
                ItemsGenerator.TestDate,
                DateTime.Now,
                DateTime.Now
            };
            return await Task.FromResult(resultList);
        }

        public async Task<List<Guid>> GuidListResult()
        {
            var resultList = new List<Guid>
            {
                ItemsGenerator.TestGuid,
                Guid.NewGuid(),
                Guid.NewGuid()
            };
            return await Task.FromResult(resultList);
        }

        public async Task<List<Item>> ItemListResult()
        {
            var items = await ItemsGenerator.GenerateItems();
            return await Task.FromResult(items);
        }

    }
}
