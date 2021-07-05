using System;

namespace Codeworx.Rest
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class ResponseTypeAttribute : Attribute
    {
        public ResponseTypeAttribute(int statusCode, Type payloadType)
        {
            StatusCode = statusCode;
            PayloadType = payloadType;
        }

        public int StatusCode { get; }

        public Type PayloadType { get; }
    }
}
