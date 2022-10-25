using System;
using System.Collections.Generic;

namespace Codeworx.Rest
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Interface, AllowMultiple = false)]
    public class RequestFormatAttribute : Attribute
    {
        public RequestFormatAttribute(string contentType, params string[] additionalContentTypes)
        {
            ContentType = contentType;
            AdditionalContentTypes = additionalContentTypes;
        }

        public string ContentType { get; }

        public IReadOnlyCollection<string> AdditionalContentTypes { get; }
    }
}
