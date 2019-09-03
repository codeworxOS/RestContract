using System;
using System.Threading.Tasks;
using Codeworx.Rest.UnitTests.Model;

namespace Codeworx.Rest.UnitTests.Api.Contract
{
    [Codeworx.Rest.RestRoute("api/MethodOverloading")]
    public interface IMethodOverloadingController
    {
        [RestGet("SameUrl")]
        Task<Item> MethodWithSameUrl1();

        [RestGet("SameUrl")]
        Task<Item> MethodWithSameUrl2();

        [RestGet("SameMethodName1")]
        Task<Item> MethodWithSameName();

        [RestGet("SameMethodName2")]
        Task<Item> MethodWithSameName(string resultItemName);
    }
}