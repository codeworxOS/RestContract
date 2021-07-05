using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Net.Mime;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Codeworx.Rest.AspNetCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Serialization;
using NJsonSchema;
using NJsonSchema.Generation.TypeMappers;
using ProtoBuf.Meta;

namespace Codeworx.Rest.UnitTests.TestServerUtilities
{
    public class Startup
    {
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            app.UseMiddleware<ExceptionMiddleware>();
            app.UseAuthentication();
#if NETCOREAPP3_1 || NET5_0
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints => endpoints.MapControllers());
            app.UseOpenApi();
#else
            app.UseMvc();
            app.UseOpenApi();
#endif
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddOpenApiDocument(options =>
                {
                    options.TypeMappers.Add(new PrimitiveTypeMapper(typeof(Stream), p =>
                    {
                        p.IsNullableRaw = false;
                        p.Format = "binary";
                        p.Type = NJsonSchema.JsonObjectType.String;
                    }));
                    options.OperationProcessors.Add(new StreamBodyOperationProcessor());
                })
#if NETCOREAPP3_1 || NET5_0
                .AddControllers(options =>
#else
                .AddMvc(options =>
#endif
            {
                options.InputFormatters.Add(new ProtobufInputFormatter(RuntimeTypeModel.Default));
                options.OutputFormatters.Add(new ProtobufOutputFormatter(RuntimeTypeModel.Default));
                options.InputFormatters.Add(new StreamInputFormatter());
            })
#if NETCOREAPP3_1 || NET5_0
                .AddNewtonsoftJson()
#endif
                .AddRestContract();

            services.AddAuthentication("TEST")
                .AddScheme<DummyAuthenticationOptions, DummyAuthenticationHandler>("TEST", p => { });

            services.AddAuthorization(p => p.AddPolicy("Default", builder =>
            {
                builder.RequireAuthenticatedUser();
            }));
        }

        private class StreamInputFormatter : InputFormatter
        {
            public StreamInputFormatter()
            {
                this.SupportedMediaTypes.Add(new Microsoft.Net.Http.Headers.MediaTypeHeaderValue(MediaTypeNames.Application.Octet));
            }

            public override IReadOnlyList<string> GetSupportedContentTypes(string contentType, Type objectType)
            {
                if (objectType == typeof(Stream))
                {
                    return base.GetSupportedContentTypes(contentType, objectType);
                }

                return ImmutableList<string>.Empty;
            }

            public override Task<InputFormatterResult> ReadRequestBodyAsync(InputFormatterContext context)
            {
                return InputFormatterResult.SuccessAsync(context.HttpContext.Request.Body);
            }

            protected override bool CanReadType(Type type)
            {
                return type == typeof(Stream);
            }
        }

        private class StreamOutputFormatter : OutputFormatter
        {
            public StreamOutputFormatter()
            {
                this.SupportedMediaTypes.Add(new Microsoft.Net.Http.Headers.MediaTypeHeaderValue(MediaTypeNames.Application.Octet));
            }

            public override Task WriteResponseBodyAsync(OutputFormatterWriteContext context)
            {
                context.HttpContext.Response.Body = (Stream)context.Object;
                return Task.CompletedTask;
            }

            protected override bool CanWriteType(Type type)
            {
                return type == typeof(Stream);
            }
        }

        private class DummyAuthenticationOptions : AuthenticationSchemeOptions
        {
        }

        private class DummyAuthenticationHandler : AuthenticationHandler<DummyAuthenticationOptions>
        {
            public DummyAuthenticationHandler(IOptionsMonitor<DummyAuthenticationOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock)
                : base(options, logger, encoder, clock)
            {
            }

            protected override Task<AuthenticateResult> HandleAuthenticateAsync()
            {
                return Task.FromResult(AuthenticateResult.NoResult());
            }
        }
    }
}