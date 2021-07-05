using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Codeworx.Rest.Authorization;

namespace Codeworx.Rest.UnitTests.Api.Contract
{
    [RestRoute("{tenant}/api/authorized")]
    public interface IExtraPathSegmentController
    {
        [RestGet("{id}")]
        Task<string> GetAsync(string id);
    }
}
