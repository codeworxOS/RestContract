using System;

namespace Codeworx.Rest
{
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
    public class RestProxyAttribute : Attribute
    {
        public RestProxyAttribute(Type contractType, Type proxyType)
        {
            ContractType = contractType;
            ProxyType = proxyType;
        }

        public Type ContractType { get; }

        public Type ProxyType { get; }
    }
}