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

        public string RoutePrefix { get; }
    }
}