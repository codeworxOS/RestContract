using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Codeworx.Rest.UnitTests.Api.Contract;

namespace Codeworx.Rest.UnitTests.Api
{
    public class AuthorizedAction : IAuthorizedAction
    {
        public Task AnonymousAsync()
        {
            return Task.CompletedTask;
        }

        public Task DenyAnonymousAsync()
        {
            return Task.CompletedTask;
        }
    }
}
