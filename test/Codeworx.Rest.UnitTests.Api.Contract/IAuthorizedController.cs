using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Codeworx.Rest.Authorization;

namespace Codeworx.Rest.UnitTests.Api.Contract
{
    [Policy("Default")]
    [RestRoute("api/authorized")]
    public interface IAuthorizedController
    {
        [Anonymous]
        [RestGet("allow")]
        Task AnonymousAsync();

        [RestGet("deny")]
        Task DenyAnonymousAsync();
    }
}
