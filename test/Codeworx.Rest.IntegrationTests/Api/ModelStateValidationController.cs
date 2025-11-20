using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Codeworx.Rest.UnitTests.Api.Contract;
using Codeworx.Rest.UnitTests.Model;

namespace Codeworx.Rest.UnitTests.Api
{
    public class ModelStateValidationController : IModelStateValidationController
    {
        public Task GuidQueryParameterAsync([QueryMember, Required] Guid id)
        {
            return Task.CompletedTask;
        }

        public Task ItemBodyNotNullAsync([BodyMember] Item item)
        {
            return Task.CompletedTask;
        }

        public Task ItemBodyNullableAsync([BodyMember] Item? item)
        {
            return Task.CompletedTask;
        }

        public Task ItemBodyNullableAttributeAsync([BodyMember(AllowNull = true)] Item item)
        {
            return Task.CompletedTask;
        }

        public Task NullableGuidQueryParameterAsync([QueryMember] Guid? id)
        {
            return Task.CompletedTask;
        }
    }
}
