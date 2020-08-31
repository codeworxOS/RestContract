using System;

namespace Codeworx.Rest.Authorization
{
    [AttributeUsage(AttributeTargets.Interface | AttributeTargets.Class | AttributeTargets.Method)]
    public class AnonymousAttribute : Attribute, IAnonymous
    {
    }
}
