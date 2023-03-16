using System;
using System.Runtime.Serialization;

namespace Codeworx.Rest.UnitTests.Model
{
    [DataContract]
    public class Item : IEquatable<Item>
    {
        [DataMember(Order = 1)]
        public Guid Id { get; set; }

        [DataMember(Order = 2)]
        public string Name { get; set; }

        [DataMember(Order = 3)]
        public int Number { get; set; }

        public bool Equals(Item other)
        {
            var isEqual = Id == other.Id
                && Number == other.Number
                && Name == other.Name;
            return isEqual;
        }
    }
}