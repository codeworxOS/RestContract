using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace System.Reflection
{
    public static class CodeworxRestTypeExtensions
    {
        private static ConcurrentDictionary<Type, Type> _lookup = new ConcurrentDictionary<Type, Type>();
        private static ConcurrentDictionary<Type, TypeConverter> _converterLookup = new ConcurrentDictionary<Type, TypeConverter>();

        public static Type GetEnumerableElementType(this Type type)
        {
            return _lookup.GetOrAdd(type, FindElementType);
        }

        public static bool HasCustomStringConverter(this Type type, out TypeConverter converter)
        {
            converter = _converterLookup.GetOrAdd(type, FindTypeConverter);

            return converter != null;
        }

        private static TypeConverter FindTypeConverter(Type type)
        {
            var converter = TypeDescriptor.GetConverter(type);
            if (converter != null
                && converter.GetType() != typeof(TypeConverter)
                && converter.CanConvertTo(typeof(string)))
            {
                return converter;
            }

            return null;
        }

        private static Type FindElementType(Type type)
        {
            if (type.IsArray)
            {
                return type.GetElementType();
            }

            return type.GetInterfaces()
               .Where(p => p.IsGenericType && p.GetGenericTypeDefinition() == typeof(IEnumerable<>))
               .Select(p => p.GetGenericArguments()[0])
               .FirstOrDefault();
        }
    }
}
