using System;

namespace Codeworx.Rest
{
    [AttributeUsage(AttributeTargets.Method)]
    public class RestOptionsAttribute : RestOperationAttribute
    {
        public RestOptionsAttribute(string template)
            : base(template)
        {
        }

        public RestOptionsAttribute()
            : base(null)
        {
        }

        public override string HttpMethod() => "OPTIONS";
    }
}