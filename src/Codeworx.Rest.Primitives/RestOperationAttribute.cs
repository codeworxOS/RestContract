using System;

namespace Codeworx.Rest
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public abstract class RestOperationAttribute : Attribute
    {
        public RestOperationAttribute(string template)
        {
            Template = template;
        }

        public RestOperationAttribute()
        {
        }

        public string Template { get; }

        public abstract string HttpMethod();
    }
}