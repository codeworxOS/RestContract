using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Codeworx.Rest.UnitTests.Model;

namespace Codeworx.Rest.UnitTests.Data
{
    public static class ItemsGenerator
    {
        public static string TestString = "Test";
        public static DateTime TestDate = new DateTime(2019, 03, 09, 12, 21, 03);
        public static Guid TestGuid = Guid.Parse("4f73dca4-86a6-4bfd-9a03-6f0e3ac254d1");
        public static int TestInt = 1;

        public static async Task<Item> GenerateItem()
        {
            var item = new Item
            {
                Id = Guid.Parse("11adeede-caa6-410a-990a-ca2e7ca10749"),
                Number = 1,
                Name = "Item 1"
            };
            return await Task.FromResult(item).ConfigureAwait(false);
        }

        public static async Task<List<Item>> GenerateItems()
        {
            var items = new List<Item>();
            var firstItem = await GenerateItem();
            items.Add(firstItem);
            items.Add(new Item
            {
                Id = Guid.Parse("64fa6119-4c01-4ef0-af3c-b14b063b3721"),
                Number = 2,
                Name = "Item 2"
            });
            items.Add(new Item
            {
                Id = Guid.Parse("3d9e3eeb-8e73-4eb8-8702-c4ca5687e142"),
                Number = 3,
                Name = "Item 3"
            });
            return await Task.FromResult(items).ConfigureAwait(false);
        }
    }
}
