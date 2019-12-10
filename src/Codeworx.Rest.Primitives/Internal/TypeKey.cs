namespace Codeworx.Rest.Internal
{
    public class TypeKey<T>
    {
        static TypeKey()
        {
            Key = new object();
        }

        public static object Key { get; }
    }
}