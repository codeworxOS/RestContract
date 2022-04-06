using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Codeworx.Rest.UnitTests.TestServerUtilities
{
    public class StartupNoExceptionMiddleware
    {
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            app.UseRouting();
            app.UseEndpoints(endpoints => endpoints.MapControllers());

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