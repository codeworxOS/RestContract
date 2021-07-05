using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Codeworx.Rest.UnitTests.Api.Contract.Model;

namespace Codeworx.Rest.UnitTests.Api.Contract
{
    [RestRoute("api/StatusCode")]
    public interface IStatusCodeController
    {
        [RestGet("Success")]
        Task<bool> GetValueSuccess();

        [RestGet("Exception")]
        Task<bool> GetValueException();


        [RestGet("Login")]
        [ResponseType((int)System.Net.HttpStatusCode.Gone, typeof(EntryNotFoundError))]
        [ResponseType((int)System.Net.HttpStatusCode.Conflict, typeof(StillInUseError))]
        Task DeleteEntry(string id);
    }
}
