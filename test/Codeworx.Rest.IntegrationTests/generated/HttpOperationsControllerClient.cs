using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Codeworx.Rest.UnitTests.Api.Contract;
using Codeworx.Rest.Client;

namespace Codeworx.Rest.UnitTests.Generated
{
    public class HttpOperationsControllerClient : RestClient<IHttpOperationsController>, IHttpOperationsController
    {
        public HttpOperationsControllerClient(RestOptions<IHttpOperationsController> options): base(options)
        {
        }

        public HttpOperationsControllerClient(RestOptions options): base(options)
        {
        }

        public Task<string> Delete()
        {
            return CallAsync(c => c.Delete());
        }

        public Task<string> Get()
        {
            return CallAsync(c => c.Get());
        }

        public Task<string> MissingOperation()
        {
            return CallAsync(c => c.MissingOperation());
        }

        public Task<string> Post()
        {
            return CallAsync(c => c.Post());
        }

        public Task<string> Put()
        {
            return CallAsync(c => c.Put());
        }
    }
}