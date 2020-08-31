using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Codeworx.Rest.Authorization;

namespace Codeworx.Rest.UnitTests.Api.Contract
{
    [RestRoute("api/authorized-action")]
    public interface IAuthorizedAction
    {
        [RestGet("allow")]
        Task AnonymousAsync();

        [RestGet("deny")]
        [Policy("Default")]
        Task DenyAnonymousAsync();
    }
}
