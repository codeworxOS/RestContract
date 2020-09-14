using System;

namespace Codeworx.Rest
{
    public class ServiceException<TPayload> : ServiceException
    {
        public ServiceException(TPayload payload)
            : base()
        {
            Payload = payload;
        }

        public TPayload Payload { get; }

        public override Type PayloadType { get; } = typeof(TPayload);

        public override object GetPayload()
        {
            return Payload;
        }
    }
}
