using System;

namespace Codeworx.Rest
{
    [AttributeUsage(AttributeTargets.Interface)]
    public class RestRouteAttribute : Attribute
    {
        public RestRouteAttribute(string routePrefix)
        {
            RoutePrefix = routePrefix;
        }

        public RestRouteAttribute(string routePrefix, string groupId)
        {
            RoutePrefix = routePrefix;
            GroupId = groupId;
        }

        public string RoutePrefix { get; }

        public string GroupId { get; }
    }
}