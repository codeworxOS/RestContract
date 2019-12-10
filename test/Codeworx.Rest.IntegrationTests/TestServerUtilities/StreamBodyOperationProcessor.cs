using System.IO;
using System.Linq;
using NJsonSchema;
using NSwag.Generation.Processors;
using NSwag.Generation.Processors.Contexts;

namespace Codeworx.Rest.UnitTests.TestServerUtilities
{
    internal class StreamBodyOperationProcessor : IOperationProcessor
    {
        public bool Process(OperationProcessorContext context)
        {
            var streamParameters = context.Parameters.Where(p => p.Key.ParameterType == typeof(Stream)).Select(p => p.Value);

            foreach (var item in streamParameters)
            {
                item.Schema = new JsonSchema
                {
                    Type = JsonObjectType.String,
                    Format = JsonFormatStrings.Binary,
                    IsNullableRaw = item.Schema.IsNullableRaw,
                };
            }

            return true;
        }
    }
}