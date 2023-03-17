using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Net.Mime;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NJsonSchema.Generation.TypeMappers;
using ProtoBuf.Meta;

namespace Codeworx.Rest.UnitTests.TestServerUtilities
{
    public class Startup
    {
        public void Configure(IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionMiddleware>();
            app.UseAuthentication();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints => endpoints.MapControllers());
            app.UseOpenApi();

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
                .AddControllers(options =>
            {
                options.InputFormatters.Add(new ProtobufInputFormatter(RuntimeTypeModel.Default));
                options.OutputFormatters.Add(new ProtobufOutputFormatter(RuntimeTypeModel.Default));
                options.InputFormatters.Add(new StreamInputFormatter());
            })
                .AddNewtonsoftJson()
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