using System;

namespace Codeworx.Rest
{
    [AttributeUsage(AttributeTargets.Method)]
    public class RestDeleteAttribute : RestOperationAttribute
    {
        public RestDeleteAttribute(string template)
            : base(template)
        {
        }

        public RestDeleteAttribute()
            : base(null)
        {
        }

        public override string HttpMethod() => "DELETE";
    }
}