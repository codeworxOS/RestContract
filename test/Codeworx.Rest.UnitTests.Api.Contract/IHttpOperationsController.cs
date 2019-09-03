using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Codeworx.Rest.UnitTests.Api.Contract
{
    [RestRoute("api/HttpOperations")]
    public interface IHttpOperationsController
    {
        [RestDelete]
        Task<string> Delete();

        [RestGet]
        Task<string> Get();

        Task<string> MissingOperation();

        [RestPost]
        Task<string> Post();

        [RestPut]
        Task<string> Put();
    }
}