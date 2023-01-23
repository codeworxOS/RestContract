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

        public ResponseTypeAttribute(int statusCode)
        {
            StatusCode = statusCode;
            PayloadType = null;
        }

        public int StatusCode { get; }

        public Type PayloadType { get; }
    }
}
