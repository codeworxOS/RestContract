using System;
using System.Runtime.Serialization;

namespace Microsoft.Extensions.DependencyInjection
{
    [DataContract(Name = nameof(DateTimeOffset))]

    public class DateTimeOffsetSurrogate
    {
        [DataMember(Order = 1)]
        public DateTime? Value { get; set; }

        [DataMember(Order = 2)]
        public TimeSpan? Offset { get; set; }

        public static implicit operator DateTimeOffset(DateTimeOffsetSurrogate surrogate)
        {
            return new DateTimeOffset(surrogate.Value.Value, surrogate.Offset.Value);
        }

        public static implicit operator DateTimeOffset?(DateTimeOffsetSurrogate surrogate)
        {
            return surrogate != null ? new DateTimeOffset(surrogate.Value.Value, surrogate.Offset.Value) : (DateTimeOffset?)null;
        }

        public static implicit operator DateTimeOffsetSurrogate(DateTimeOffset source)
        {
            return new DateTimeOffsetSurrogate
            {
                Value = source.DateTime,
                Offset = source.Offset,
            };
        }

        public static implicit operator DateTimeOffsetSurrogate(DateTimeOffset? source)
        {
            return new DateTimeOffsetSurrogate
            {
                Value = source?.DateTime,
                Offset = source?.Offset,
            };
        }
    }
}