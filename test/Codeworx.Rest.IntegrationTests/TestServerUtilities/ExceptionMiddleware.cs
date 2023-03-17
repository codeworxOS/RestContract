using System;
using System.Net;
using System.Net.Mime;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Codeworx.Rest.UnitTests.TestServerUtilities
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await this.next(httpContext);
            }
            catch (OperationCanceledException)
            {
                httpContext.Response.StatusCode = 499; // client closed request
                /* test server does not handle cancellation correctly https://github.com/dotnet/aspnetcore/issues/5938
                 * rethrow exception to receive a OperationCanceledException on client.SendAsync(request, token) */
                throw; 
            }
            catch (Exception)
            {
                httpContext.Response.ContentType = MediaTypeNames.Text.Plain;
                httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                await httpContext.Response.WriteAsync("Internal server error!");
            }
        }
    }
}