using Microsoft.CodeAnalysis;

namespace Codeworx.Rest.Tool.Extensions
{
    public static class SyntaxNodeExtensions
    {
        public static TNode FindFirstParent<TNode>(this SyntaxNode node)
            where TNode : SyntaxNode
        {
            while (node.Parent != null)
            {
                if (node.Parent is TNode result)
                {
                    return result;
                }

                node = node.Parent;
            }

            return null;
        }
    }
}