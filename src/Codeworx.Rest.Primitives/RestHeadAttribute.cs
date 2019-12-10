using System;

namespace Codeworx.Rest
{
    [AttributeUsage(AttributeTargets.Method)]
    public class RestHeadAttribute : RestOperationAttribute
    {
        public RestHeadAttribute(string template)
            : base(template)
        {
        }

        public RestHeadAttribute()
            : base(null)
        {
        }

        public override string HttpMethod() => "HEAD";
    }
}