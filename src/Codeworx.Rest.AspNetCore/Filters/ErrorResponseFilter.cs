using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace Codeworx.Rest.AspNetCore.Filters
{
    public class ErrorResponseFilter : IAsyncExceptionFilter
    {
        public Task OnExceptionAsync(ExceptionContext context)
        {
            var dispatcher = context.HttpContext.RequestServices.GetRequiredService<IServiceErrorDispatcher>();

            if (dispatcher.CanHandle(context.Exception))
            {
                var supportedResponseTypes = context.ActionDescriptor.EndpointMetadata.OfType<ResponseTypeAttribute>()
                                                .ToDictionary(p => p.PayloadType, p => p.StatusCode);

                var payloadType = dispatcher.GetPayloadType(context.Exception);
                if (supportedResponseTypes.TryGetValue(payloadType, out var statusCode))
                {
                    var contentResult = new ObjectResult(dispatcher.GetPayload(context.Exception));
                    contentResult.StatusCode = statusCode;
                    context.Result = contentResult;
                    context.ExceptionHandled = true;
                }
            }

            return Task.CompletedTask;
        }
    }
}