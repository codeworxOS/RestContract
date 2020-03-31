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
    public class ProtobufInputFormatter : InputFormatter
    {
        private readonly TypeModel _typeModel;

        static ProtobufInputFormatter()
        {
            ContentType = Constants.ProtobufMimeType;
        }

        public ProtobufInputFormatter(TypeModel typeModel)
        {
            SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse(ContentType));
            this.SupportedMediaTypes.Add(ContentType);
            _typeModel = typeModel;
        }

        public static string ContentType { get; }

        public override async Task<InputFormatterResult> ReadAsync(InputFormatterContext context)
        {
            var request = context.HttpContext.Request;

            if (request.Headers.TryGetValue(Constants.ProtobufNullHeader, out var values))
            {
                if (values[0].Equals("true", StringComparison.OrdinalIgnoreCase))
                {
                    return await InputFormatterResult.SuccessAsync(null);
                }
            }

            using (var ms = new MemoryStream())
            {
                await request.Body.CopyToAsync(ms);
                ms.Seek(0, SeekOrigin.Begin);

                var model = _typeModel.Deserialize(ms, null, context.ModelType);
                return await InputFormatterResult.SuccessAsync(model);
            }
        }

        public override Task<InputFormatterResult> ReadRequestBodyAsync(InputFormatterContext context)
        {
            throw new NotImplementedException();
        }

        protected override bool CanReadType(Type type)
        {
            return _typeModel.CanSerialize(type);
        }
    }
}