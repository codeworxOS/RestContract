using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Codeworx.Rest.UnitTests.Api.Contract;
using Codeworx.Rest.UnitTests.Model;

namespace Codeworx.Rest.UnitTests.Api
{
    public class SerializeParameterController : ISerializeParameterController
    {
        public async Task<string> GetStringQueryParameter(string parameter)
        {
            return await Task.FromResult(parameter);
        }

        public async Task<string> GetStringUrlParameter(string parameter)
        {
            // aspnetcore #11544 - %2F in route parameter is not decoded by model binder.
            parameter = parameter != null ? Uri.UnescapeDataString(parameter) : null;
            return await Task.FromResult(parameter);
        }

        public async Task<string> GetStringBodyParameter(string parameter)
        {
            return await Task.FromResult(parameter);
        }

        public async Task<DateTime?> GetDateTimeQueryParameter(DateTime? parameter)
        {
            return await Task.FromResult(parameter);
        }

        public async Task<DateTime?> GetDateTimeUrlParameter(DateTime? parameter)
        {
            return await Task.FromResult(parameter);
        }

        public async Task<DateTime?> GetDateTimeBodyParameter(DateTime? parameter)
        {
            return await Task.FromResult(parameter);
        }

        public async Task<DateTimeOffset?> GetDateTimeOffsetQueryParameter(DateTimeOffset? parameter)
        {
            return await Task.FromResult(parameter);
        }

        public async Task<DateTimeOffset?> GetDateTimeOffsetUrlParameter(DateTimeOffset? parameter)
        {
            return await Task.FromResult(parameter);
        }

        public async Task<DateTimeOffset?> GetDateTimeOffsetBodyParameter(DateTimeOffset? parameter)
        {
            return await Task.FromResult(parameter);
        }

        public async Task<Guid?> GetGuidQueryParameter(Guid? parameter)
        {
            return await Task.FromResult(parameter);
        }

        public async Task<Guid?> GetGuidUrlParameter(Guid? parameter)
        {
            return await Task.FromResult(parameter);
        }

        public async Task<Guid?> GetGuidBodyParameter(Guid? parameter)
        {
            return await Task.FromResult(parameter);
        }


        public async Task<int?> GetIntQueryParameter(int? parameter)
        {
            return await Task.FromResult(parameter);
        }

        public async Task<int?> GetIntUrlParameter(int? parameter)
        {
            return await Task.FromResult(parameter);
        }

        public async Task<int?> GetIntBodyParameter(int? parameter)
        {
            return await Task.FromResult(parameter);
        }

        public async Task<decimal?> GetDecimalQueryParameter(decimal? parameter)
        {
            return await Task.FromResult(parameter);
        }

        public async Task<decimal?> GetDecimalUrlParameter(decimal? parameter)
        {
            return await Task.FromResult(parameter);
        }

        public async Task<decimal?> GetDecimalBodyParameter(decimal? parameter)
        {
            return await Task.FromResult(parameter);
        }

        public async Task<double?> GetDoubleQueryParameter(double? parameter)
        {
            return await Task.FromResult(parameter);
        }

        public async Task<double?> GetDoubleUrlParameter(double? parameter)
        {
            return await Task.FromResult(parameter);
        }

        public async Task<double?> GetDoubleBodyParameter(double? parameter)
        {
            return await Task.FromResult(parameter);
        }

        public async Task<float?> GetFloatQueryParameter(float? parameter)
        {
            return await Task.FromResult(parameter);
        }

        public async Task<float?> GetFloatUrlParameter(float? parameter)
        {
            return await Task.FromResult(parameter);
        }

        public async Task<float?> GetFloatBodyParameter(float? parameter)
        {
            return await Task.FromResult(parameter);
        }

        public async Task<List<Guid>> GetGuidListQueryParameter(List<Guid> parameter)
        {
            return await Task.FromResult(parameter);
        }

        public async Task<List<Guid>> GetGuidListUrlParameter(List<Guid> parameter)
        {
            return await Task.FromResult(parameter);
        }

        public async Task<List<Guid>> GetGuidListBodyParameter(int id, List<Guid> parameter)
        {
            return await Task.FromResult(parameter);
        }

        public async Task<Item> GetItemBodyParameter(Item parameter)
        {
            return await Task.FromResult(parameter);
        }

        public async Task<Item> GetItemBodyParameterWithNoAttribute(Item parameter)
        {
            return await Task.FromResult(parameter);
        }

        public async Task<Item> GetItemBodyParameterAsLastParameter(Guid id, string name, Item parameter)
        {
            return await Task.FromResult(parameter);
        }

        public Task<DateTime?> GetDateTimeQueryExplicitParameter([QueryMember] DateTime? parameter)
        {
            return Task.FromResult(parameter);
        }

        public Task<DateTimeOffset?> GetDateTimeOffsetQueryExplicitParameter([QueryMember] DateTimeOffset? parameter)
        {
            return Task.FromResult(parameter);
        }

        public Task<decimal?> GetDecimalQueryExplicitParameter([QueryMember] decimal? parameter)
        {
            return Task.FromResult(parameter);
        }

        public Task<double?> GetDoubleQueryExplicitParameter([BodyMember] double? parameter)
        {
            return Task.FromResult(parameter);
        }

        public Task<float?> GetFloatQueryExplicitParameter([QueryMember] float? parameter)
        {
            return Task.FromResult(parameter);
        }

        public Task<List<Guid>> GetGuidListQueryExplicitParameter([QueryMember] List<Guid> parameter)
        {
            return Task.FromResult(parameter);
        }

        public Task<Guid?> GetGuidQueryExplicitParameter(Guid? parameter)
        {
            return Task.FromResult(parameter);
        }

        public Task<int?> GetIntQueryExplicitParameter(int? parameter)
        {
            return Task.FromResult(parameter);
        }

        public Task<string> GetStringQueryExplicitParameter([QueryMember] string parameter)
        {
            return Task.FromResult(parameter);
        }
    }
}
