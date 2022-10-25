using System;
using System.Collections.Generic;

namespace Codeworx.Rest
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Interface, AllowMultiple = false)]
    public class ResponseFormatAttribute : Attribute
    {
        public ResponseFormatAttribute(string contentType, params string[] additionalContentTypes)
        {
            ContentType = contentType;
            AdditionalContentTypes = additionalContentTypes;
        }

        public string ContentType { get; }

        public IReadOnlyCollection<string> AdditionalContentTypes { get; }
    }
}
