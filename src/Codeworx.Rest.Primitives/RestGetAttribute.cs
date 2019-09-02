using System;

namespace Codeworx.Rest
{
    [AttributeUsage(AttributeTargets.Method)]
    public class RestGetAttribute : RestOperationAttribute
    {
        public RestGetAttribute(string template)
            : base(template)
        {
        }

        public RestGetAttribute()
            : base(null)
        {
        }

        public override string HttpMethod() => "GET";
    }
}