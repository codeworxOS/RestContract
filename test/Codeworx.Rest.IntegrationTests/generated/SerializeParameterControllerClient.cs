using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Codeworx.Rest.UnitTests.Model;
using Codeworx.Rest.UnitTests.Api.Contract;
using Codeworx.Rest.Client;

namespace Codeworx.Rest.UnitTests.Generated
{
    public class SerializeParameterControllerClient : RestClient<ISerializeParameterController>, ISerializeParameterController
    {
        public SerializeParameterControllerClient(RestOptions<ISerializeParameterController> options): base(options)
        {
        }

        public SerializeParameterControllerClient(RestOptions options): base(options)
        {
        }

        public Task<string> GetStringQueryParameter(string parameter)
        {
            return CallAsync(c => c.GetStringQueryParameter(parameter));
        }

        public Task<string> GetStringUrlParameter(string parameter)
        {
            return CallAsync(c => c.GetStringUrlParameter(parameter));
        }

        public Task<string> GetStringBodyParameter(string parameter)
        {
            return CallAsync(c => c.GetStringBodyParameter(parameter));
        }

        public Task<DateTime?> GetDateTimeQueryParameter(DateTime? parameter)
        {
            return CallAsync(c => c.GetDateTimeQueryParameter(parameter));
        }

        public Task<DateTime?> GetDateTimeUrlParameter(DateTime? parameter)
        {
            return CallAsync(c => c.GetDateTimeUrlParameter(parameter));
        }

        public Task<DateTime?> GetDateTimeBodyParameter(DateTime? parameter)
        {
            return CallAsync(c => c.GetDateTimeBodyParameter(parameter));
        }

        public Task<Guid?> GetGuidQueryParameter(Guid? parameter)
        {
            return CallAsync(c => c.GetGuidQueryParameter(parameter));
        }

        public Task<Guid?> GetGuidUrlParameter(Guid? parameter)
        {
            return CallAsync(c => c.GetGuidUrlParameter(parameter));
        }

        public Task<Guid?> GetGuidBodyParameter(Guid? parameter)
        {
            return CallAsync(c => c.GetGuidBodyParameter(parameter));
        }

        public Task<int?> GetIntQueryParameter(int? parameter)
        {
            return CallAsync(c => c.GetIntQueryParameter(parameter));
        }

        public Task<int?> GetIntUrlParameter(int? parameter)
        {
            return CallAsync(c => c.GetIntUrlParameter(parameter));
        }

        public Task<int?> GetIntBodyParameter(int? parameter)
        {
            return CallAsync(c => c.GetIntBodyParameter(parameter));
        }

        public Task<List<Guid>> GetGuidListQueryParameter(List<Guid> parameter)
        {
            return CallAsync(c => c.GetGuidListQueryParameter(parameter));
        }

        public Task<List<Guid>> GetGuidListUrlParameter(List<Guid> parameter)
        {
            return CallAsync(c => c.GetGuidListUrlParameter(parameter));
        }

        public Task<List<Guid>> GetGuidListBodyParameter(List<Guid> parameter)
        {
            return CallAsync(c => c.GetGuidListBodyParameter(parameter));
        }

        public Task<Item> GetItemBodyParameter(Item parameter)
        {
            return CallAsync(c => c.GetItemBodyParameter(parameter));
        }

        public Task<Item> GetItemBodyParameterWithNoAttribute(Item parameter)
        {
            return CallAsync(c => c.GetItemBodyParameterWithNoAttribute(parameter));
        }

        public Task<Item> GetItemBodyParameterAsLastParameter(Guid id, string name, Item parameter)
        {
            return CallAsync(c => c.GetItemBodyParameterAsLastParameter(id, name, parameter));
        }
    }
}