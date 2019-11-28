using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Codeworx.Rest.UnitTests.Api.Contract;
using Microsoft.AspNetCore.Mvc;

namespace Codeworx.Rest.UnitTests.Api
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public class HttpOperationsController : IHttpOperationsController
    {
        public async Task<string> Delete()
        {
            var methodName = await GetMethodName();
            return methodName;
        }

        public async Task<string> Get()
        {
            var methodName = await GetMethodName();
            return methodName;
        }

        public async Task<string> MissingOperation()
        {
            var methodName = await GetMethodName();
            return methodName;
        }

        public async Task<string> Post()
        {
            var methodName = await GetMethodName();
            return methodName;
        }

        public async Task<string> Put()
        {
            var methodName = await GetMethodName();
            return methodName;
        }

        private async Task<string> GetMethodName([CallerMemberName] string callerName = "")
        {
            return await Task.FromResult(callerName);
        }
    }
}