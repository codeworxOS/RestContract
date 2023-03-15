using Microsoft.Extensions.DependencyInjection;

namespace Codeworx.Rest.Client.Builder
{
    public class GroupRestOptionsBuilder : IGroupRestOptionsBuilder
    {
        public GroupRestOptionsBuilder(IServiceCollection services, string groupId)
        {
            Services = services;
            GroupId = groupId;
        }

        public IServiceCollection Services { get; }

        public string GroupId { get; }
    }
}