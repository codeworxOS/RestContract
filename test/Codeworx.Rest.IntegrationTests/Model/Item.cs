using System;
using System.Collections.Generic;

namespace Codeworx.Rest.UnitTests.Model
{
    public class Item : IEquatable<Item>
    {
        public Guid Id { get; set; }
        public int Number { get; set; }
        public string Name { get; set; }

        public bool Equals(Item other)
        {
            var isEqual = Id == other.Id
                && Number == other.Number
                && Name == other.Name;
            return isEqual;
        }
    }
}
