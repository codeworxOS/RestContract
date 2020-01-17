using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Codeworx.Rest.Formatters.Protobuf;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;
using ProtoBuf.Meta;

namespace Codeworx.Rest.UnitTests.TestServerUtilities
{
    public class ProtobufOutputFormatter : OutputFormatter
    {
        private readonly TypeModel _typeModel;

        static ProtobufOutputFormatter()
        {
            ContentType = Constants.ProtobufMimeType;
        }

        public ProtobufOutputFormatter(TypeModel typeModel)
        {
            SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse(ContentType));
            this.SupportedMediaTypes.Add(ContentType);
            _typeModel = typeModel;
        }

        public static string ContentType { get; }

        public override async Task WriteResponseBodyAsync(OutputFormatterWriteContext context)
        {
            var response = context.HttpContext.Response;
            using (var ms = new MemoryStream())
            {
                _typeModel.Serialize(ms, context.Object);
                ms.Seek(0, SeekOrigin.Begin);

                await ms.CopyToAsync(response.Body);

            }
        }
    }
}