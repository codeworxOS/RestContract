using System;

namespace Codeworx.Rest
{
    [AttributeUsage(AttributeTargets.Method)]
    public class RestPostAttribute : RestOperationAttribute
    {
        public RestPostAttribute(string template)
            : base(template)
        {
        }

        public RestPostAttribute()
            : base(null)
        {
        }

        public override string HttpMethod() => "POST";
    }
}