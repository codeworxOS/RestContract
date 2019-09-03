using System.Threading.Tasks;
using Codeworx.Rest.Client;
using Codeworx.Rest.UnitTests.Api.Contract;

namespace Codeworx.Rest.UnitTests.Dao
{
    class HttpOperationsDao : RestClient<IHttpOperationsController>, IHttpOperationsController
    {
        public HttpOperationsDao(RestOptions options)
            : base(options)
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
