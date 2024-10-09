using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Net.Mime;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NJsonSchema.Generation.TypeMappers;
using NSwag.Generation.Processors;
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
                    options.SchemaSettings.TypeMappers.Add(new PrimitiveTypeMapper(typeof(Stream), p =>
                    {
                        p.IsNullableRaw = false;
                        p.Format = "binary";
                        p.Type = NJsonSchema.JsonObjectType.String;
                    }));
                    options.OperationProcessors.Add(new StreamBodyOperationProcessor());
                    options.OperationProcessors.Add(new ProducesConsumesProcessor());
                })
                .AddControllers(options =>
            {
                options.InputFormatters.Add(new ProtobufInputFormatter(RuntimeTypeModel.Default));
                options.OutputFormatters.Add(new ProtobufOutputFormatter(RuntimeTypeModel.Default));
                options.InputFormatters.Add(new StreamInputFormatter());
                options.InputFormatters.Add(new PlainTextInputFormatter());
            })
                .AddRestContract();

            services.AddAuthentication("TEST")
                .AddScheme<DummyAuthenticationOptions, DummyAuthenticationHandler>("TEST", p => { });

            services.AddAuthorization(p => p.AddPolicy("Default", builder =>
            {
                builder.RequireAuthenticatedUser();
            }));
        }

        private class PlainTextInputFormatter : InputFormatter
        {
            public PlainTextInputFormatter()
            {
                this.SupportedMediaTypes.Add(new Microsoft.Net.Http.Headers.MediaTypeHeaderValue(MediaTypeNames.Text.Plain));
            }

            public override IReadOnlyList<string> GetSupportedContentTypes(string contentType, Type objectType)
            {
                if (objectType == typeof(string) || objectType == typeof(Uri))
                {
                    return base.GetSupportedContentTypes(contentType, objectType);
                }

                return ImmutableList<string>.Empty;
            }

            protected override bool CanReadType(Type type)
            {
                return type == typeof(string) || type == typeof(Uri);
            }


            public override async Task<InputFormatterResult> ReadRequestBodyAsync(InputFormatterContext context)
            {
                using (var ms = new MemoryStream())
                {
                    await context.HttpContext.Request.Body.CopyToAsync(ms);

                    if (context.ModelType == typeof(string))
                    {
                        return await InputFormatterResult.SuccessAsync(Encoding.UTF8.GetString(ms.ToArray()));
                    }
                    else if (context.ModelType == typeof(Uri))
                    {
                        return await InputFormatterResult.SuccessAsync(new Uri(Encoding.UTF8.GetString(ms.ToArray())));
                    }
                }

                return InputFormatterResult.NoValue();
            }
        }

        private class StreamInputFormatter : InputFormatter
        {
            public StreamInputFormatter()
            {
                this.SupportedMediaTypes.Add(new Microsoft.Net.Http.Headers.MediaTypeHeaderValue(MediaTypeNames.Application.Octet));
                this.SupportedMediaTypes.Add(new Microsoft.Net.Http.Headers.MediaTypeHeaderValue(MediaTypeNames.Application.Pdf));
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

        private class DummyAuthenticationOptions : AuthenticationSchemeOptions
        {
        }

        private class DummyAuthenticationHandler : AuthenticationHandler<DummyAuthenticationOptions>
        {
#if NET8_0_OR_GREATER
            public DummyAuthenticationHandler(IOptionsMonitor<DummyAuthenticationOptions> options, ILoggerFactory logger, UrlEncoder encoder)
                : base(options, logger, encoder)
            {
            }
#else
            public DummyAuthenticationHandler(IOptionsMonitor<DummyAuthenticationOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock)
                : base(options, logger, encoder, clock)
            {
            }
#endif

            protected override Task<AuthenticateResult> HandleAuthenticateAsync()
            {
                return Task.FromResult(AuthenticateResult.NoResult());
            }
        }
    }
}