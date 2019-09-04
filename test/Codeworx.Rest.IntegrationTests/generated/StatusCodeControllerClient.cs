using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Codeworx.Rest.UnitTests.Api.Contract;
using Codeworx.Rest.Client;

namespace Codeworx.Rest.UnitTests.Generated
{
    public class StatusCodeControllerClient : RestClient<IStatusCodeController>, IStatusCodeController
    {
        public StatusCodeControllerClient(RestOptions<IStatusCodeController> options): base(options)
        {
        }

        public StatusCodeControllerClient(RestOptions options): base(options)
        {
        }

        public Task<bool> GetValueSuccess()
        {
            return CallAsync(c => c.GetValueSuccess());
        }

        public Task<bool> GetValueException()
        {
            return CallAsync(c => c.GetValueException());
        }
    }
}