using System;
using System.Threading.Tasks;
using Codeworx.Rest.Client;
using Codeworx.Rest.UnitTests.Api.Contract;
using Codeworx.Rest.UnitTests.Model;

namespace Codeworx.Rest.UnitTests.Dao
{
    public class MethodOverloadingDao : RestClient<IMethodOverloadingController>, IMethodOverloadingController
    {
        public MethodOverloadingDao(RestOptions options)
            : base(options)
        {
        }

        public Task<Item> MethodWithSameName()
        {
            return CallAsync(c => c.MethodWithSameName());
        }

        public Task<Item> MethodWithSameName(Guid id)
        {
            return CallAsync(c => c.MethodWithSameName(id));
        }

        public Task<Item> MethodWithSameUrl1()
        {
            return CallAsync(c => c.MethodWithSameUrl1());
        }

        public Task<Item> MethodWithSameUrl2()
        {
            return CallAsync(c => c.MethodWithSameUrl2());
        }
    }
}
