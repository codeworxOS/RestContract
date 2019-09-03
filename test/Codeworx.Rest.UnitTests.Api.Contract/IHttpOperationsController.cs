using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Codeworx.Rest.UnitTests.Api.Contract
{
    [RestRoute("api/HttpOperations")]
    public interface IHttpOperationsController
    {
        [RestGet]
        Task<string> Get();

        [RestPut]
        Task<string> Put();

        [RestPost]
        Task<string> Post();

        [RestDelete]
        Task<string> Delete();

        Task<string> MissingOperation();
    }
}
