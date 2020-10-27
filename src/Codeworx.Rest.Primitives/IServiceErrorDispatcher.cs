using System;

namespace Codeworx.Rest
{
    public interface IServiceErrorDispatcher
    {
        bool CanHandle(Exception ex);

        object GetPayload(Exception ex);

        Type GetPayloadType(Exception ex);

        Exception GetException<TPayload>(TPayload payload);
    }
}