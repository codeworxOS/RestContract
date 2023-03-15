using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Codeworx.Rest.UnitTests.TestServerUtilities
{
    public class StartupNoExceptionMiddleware
    {
        public void Configure(IApplicationBuilder app)
        {
            app.UseRouting();
            app.UseEndpoints(endpoints => endpoints.MapControllers());

        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvcCore()
                .AddRestContract()
                ;
        }
    }
}