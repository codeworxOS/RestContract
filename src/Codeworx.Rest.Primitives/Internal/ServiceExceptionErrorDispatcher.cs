using System;

namespace Codeworx.Rest.Internal
{
    public class ServiceExceptionErrorDispatcher : IServiceErrorDispatcher
    {
        public bool CanHandler(Exception ex)
        {
            return ex is ServiceException;
        }

        public Exception GetException<TPayload>(TPayload payload)
        {
            return new ServiceException<TPayload>(payload);
        }

        public object GetPayload(Exception ex)
        {
            return ((ServiceException)ex).GetPayload();
        }

        public Type GetPayloadType(Exception ex)
        {
            return ((ServiceException)ex).PayloadType;
        }
    }
}
