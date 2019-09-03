using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Codeworx.Rest.UnitTests.Api.Contract;
using Microsoft.AspNetCore.Http;

namespace Codeworx.Rest.UnitTests.Api
{
    public class StatusCodeController : IStatusCodeController
    {
        public async Task<bool> GetValueSuccess()
        {
            return await Task.FromResult(true);
        }


        public async Task<bool> GetValueException()
        {
            throw new Exception();
        }
    }
}
