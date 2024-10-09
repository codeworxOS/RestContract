using System.Linq;
using Microsoft.AspNetCore.Mvc;
using NSwag.Generation.AspNetCore;
using NSwag.Generation.Processors;
using NSwag.Generation.Processors.Contexts;

namespace Codeworx.Rest.UnitTests.TestServerUtilities
{
    internal class ProducesConsumesProcessor : IOperationProcessor
    {
        public bool Process(OperationProcessorContext context)
        {
            if (context is AspNetCoreOperationProcessorContext aspContext)
            {
                foreach (var item in aspContext.ApiDescription.SupportedResponseTypes)
                {
                    if (context.OperationDescription.Operation.Responses.TryGetValue($"{item.StatusCode}", out var response) && response.Content.Any())
                    {
                        var type = response.Content.First().Value;
                        response.Content.Clear();
                        foreach (var format in item.ApiResponseFormats.Select(p => p.MediaType).Distinct().ToList())
                        {
                            response.Content.Add(format, type);
                        }
                    }
                }
            }


            if (context.OperationDescription.Operation.ActualConsumes.Any() && context.OperationDescription.Operation.RequestBody.Content.Any())
            {
                var bodyType = context.OperationDescription.Operation.RequestBody.Content.First().Value;

                context.OperationDescription.Operation.RequestBody.Content.Clear();

                foreach (var item in context.OperationDescription.Operation.ActualConsumes)
                {
                    context.OperationDescription.Operation.RequestBody.Content.Add(item, bodyType);
                }

            }

            return true;
        }
    }
}