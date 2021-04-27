using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Codeworx.Rest.UnitTests.Api
{
    [Route("api/[controller]")]
    [Authorize("Default")]
    public class NoContractController
    {
        [HttpGet("allow")]
        [AllowAnonymous]
        public Task<string> AnonymousAsync()
        {
            return Task.FromResult("allow");
        }

        [HttpGet("deny")]
        public Task<string> DenyAnonymousAsync()
        {
            return Task.FromResult("deny");
        }
    }
}
