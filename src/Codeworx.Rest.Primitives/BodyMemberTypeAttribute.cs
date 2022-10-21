using System;
using System.Collections.Generic;

namespace Codeworx.Rest
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class BodyMemberTypeAttribute : Attribute
    {
        public BodyMemberTypeAttribute(string contentType, params string[] additionalContentTypes)
        {
            ContentType = contentType;
            AdditionalContentTypes = additionalContentTypes;
        }

        public string ContentType { get; }

        public IReadOnlyCollection<string> AdditionalContentTypes { get; }
    }
}
