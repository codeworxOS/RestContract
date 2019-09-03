using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Codeworx.Rest.Client;
using Codeworx.Rest.UnitTests.Api.Contract;

namespace Codeworx.Rest.UnitTests.Dao
{
    public class StatusCodeDao : RestClient<IStatusCodeController>, IStatusCodeController
    {
        public StatusCodeDao(RestOptions options)
            : base(options)
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
