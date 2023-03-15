using System;
using System.Threading.Tasks;
using Codeworx.Rest.UnitTests.Api.Contract;
using Codeworx.Rest.UnitTests.Api.Contract.Model;

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
            await Task.Yield();
            throw new Exception();
        }

        public Task DeleteEntry(string id)
        {
            if (id == "1")
            {
                throw new ServiceException<EntryNotFoundError>(new EntryNotFoundError());
            }
            else if (id == "2")
            {
                throw new ServiceException<StillInUseError>(new StillInUseError { BlockingResource = "Invoice" });
            }

            return Task.CompletedTask;
        }
    }
}
