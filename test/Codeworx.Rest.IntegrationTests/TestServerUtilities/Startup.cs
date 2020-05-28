using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Net.Mime;
using System.Threading.Tasks;
using Codeworx.Rest.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
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
#if NETCOREAPP3_1
            app.UseRouting();
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
#if NETCOREAPP3_1
                .AddControllers(options =>
#else
                .AddMvcCore(options =>
#endif
            {
                options.InputFormatters.Add(new ProtobufInputFormatter(RuntimeTypeModel.Default));
                options.OutputFormatters.Add(new ProtobufOutputFormatter(RuntimeTypeModel.Default));
                options.InputFormatters.Add(new StreamInputFormatter());
            })
#if NETCOREAPP3_1
                .AddNewtonsoftJson()
#else
                .AddApiExplorer()
                .AddJsonFormatters(options => options.ContractResolver = new CamelCasePropertyNamesContractResolver())
#endif
                .AddRestContract()
                ;
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
    }
}