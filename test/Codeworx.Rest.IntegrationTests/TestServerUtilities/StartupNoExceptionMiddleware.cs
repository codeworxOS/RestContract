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
#if NETCOREAPP3_1
            app.UseRouting();
            app.UseEndpoints(endpoints => endpoints.MapControllers());
#else
            app.UseMvc();
#endif
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvcCore()
                .AddRestContract()
#if NETCOREAPP2_1
                .AddJsonFormatters(options => options.ContractResolver = new CamelCasePropertyNamesContractResolver())
#endif
                ;
        }
    }
}