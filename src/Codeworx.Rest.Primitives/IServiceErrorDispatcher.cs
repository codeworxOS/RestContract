using System;

namespace Codeworx.Rest
{
    public interface IServiceErrorDispatcher
    {
        bool CanHandler(Exception ex);

        object GetPayload(Exception ex);

        Type GetPayloadType(Exception ex);

        Exception GetException<TPayload>(TPayload payload);
    }
}
