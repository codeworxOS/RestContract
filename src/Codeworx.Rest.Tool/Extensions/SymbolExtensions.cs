using Microsoft.CodeAnalysis;

namespace Codeworx.Rest.Tool.Extensions
{
    public static class SymbolExtensions
    {
        public static string AssemblyQualifiedName(this ISymbol symbol)
        {
            var assemblyQualifiedName = symbol.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
            return assemblyQualifiedName;
        }
    }
}