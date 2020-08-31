using System;

namespace Codeworx.Rest.Authorization
{
    [AttributeUsage(AttributeTargets.Interface | AttributeTargets.Class | AttributeTargets.Method)]
    public class PolicyAttribute : Attribute, IPolicy
    {
        public PolicyAttribute(string name)
        {
            Name = name;
        }

        public string Name { get; }
    }
}
