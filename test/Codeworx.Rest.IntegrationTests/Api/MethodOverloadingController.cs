using System;
using System.Threading.Tasks;
using Codeworx.Rest.UnitTests.Api.Contract;
using Codeworx.Rest.UnitTests.Data;
using Codeworx.Rest.UnitTests.Model;
using Microsoft.AspNetCore.Mvc;

namespace Codeworx.Rest.UnitTests.Api
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public class MethodOverloadingController : IMethodOverloadingController
    {
        public async Task<Item> MethodWithSameName()
        {
            var item = await ItemsGenerator.GenerateItem().ConfigureAwait(false);
            return item;
        }

        public async Task<Item> MethodWithSameName(string resultItemName)
        {
            var item = await ItemsGenerator.GenerateItem().ConfigureAwait(false);
            item.Name = resultItemName;
            return item;
        }

        public async Task<Item> MethodWithSameUrl1()
        {
            var item = await ItemsGenerator.GenerateItem().ConfigureAwait(false);
            return item;
        }

        public async Task<Item> MethodWithSameUrl2()
        {
            var item = await ItemsGenerator.GenerateItem().ConfigureAwait(false);
            return item;
        }
    }
}