using System;

namespace Codeworx.Rest
{
    [AttributeUsage(AttributeTargets.Parameter)]
    public class BodyMemberAttribute : Attribute
    {
        public BodyMemberAttribute()
        {
            ContentTypes = new string[] { };
        }

        public BodyMemberAttribute(string contentType, params string[] additionalContentTypes)
        {
            ContentTypes = new string[additionalContentTypes.Length + 1];
            ContentTypes[0] = contentType;
            Array.Copy(additionalContentTypes, 0, ContentTypes, 1, additionalContentTypes.Length);
        }

        public string[] ContentTypes { get; }
    }
}