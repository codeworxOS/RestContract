using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Codeworx.Rest.UnitTests.Api.Contract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace Codeworx.Rest.UnitTests.Api
{
    public class ExtraPathSegmentController : ControllerBase, IExtraPathSegmentController
    {
        public Task<string> GetAsync(string id)
        {
            var result = $"{this.HttpContext.GetRouteValue("tenant")}-{id}";

            return Task.FromResult(result);
        }
    }
}
