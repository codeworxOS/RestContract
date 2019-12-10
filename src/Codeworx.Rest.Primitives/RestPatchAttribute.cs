using System;

namespace Codeworx.Rest
{
    [AttributeUsage(AttributeTargets.Method)]
    public class RestPatchAttribute : RestOperationAttribute
    {
        public RestPatchAttribute(string template)
            : base(template)
        {
        }

        public RestPatchAttribute()
            : base(null)
        {
        }

        public override string HttpMethod() => "PATCH";
    }
}