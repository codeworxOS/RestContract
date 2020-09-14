using System;

namespace Codeworx.Rest
{
    public abstract class ServiceException : Exception
    {
        public ServiceException()
            : base($"Service exception thrown:")
        {
        }

        public abstract Type PayloadType { get; }

        public abstract object GetPayload();
    }
}
