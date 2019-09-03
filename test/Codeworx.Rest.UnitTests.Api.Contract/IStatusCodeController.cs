using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Codeworx.Rest.UnitTests.Api.Contract
{
    [RestRoute("api/StatusCode")]
    public interface IStatusCodeController
    {
        [RestGet("Success")]
        Task<bool> GetValueSuccess();

        [RestGet("Exception")]
        Task<bool> GetValueException();
    }
}
