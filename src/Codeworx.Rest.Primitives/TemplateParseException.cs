using System;

namespace Codeworx.Rest
{
    public class TemplateParseException : Exception
    {
        public TemplateParseException()
            : base("Error parsing uri template")
        {
        }

        public TemplateParseException(string message)
            : base(message)
        {
        }

        public TemplateParseException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}