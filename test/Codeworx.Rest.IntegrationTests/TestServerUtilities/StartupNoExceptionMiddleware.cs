using Codeworx.Rest.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Serialization;

namespace Codeworx.Rest.UnitTests.TestServerUtilities
{
    public class StartupNoExceptionMiddleware
    {
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            app.UseMvc();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvcCore()
                .AddRestContract()
                .AddJsonFormatters(options => options.ContractResolver = new CamelCasePropertyNamesContractResolver());
        }
    }
}