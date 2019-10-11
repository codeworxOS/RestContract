using System;
using System.Collections.Generic;
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

        public override Task<InputFormatterResult> ReadRequestBodyAsync(InputFormatterContext context)
        {
            var request = context.HttpContext.Request;

            if (request.Headers.TryGetValue(Constants.ProtobufNullHeader, out var values))
            {
                if (values[0].Equals("true", StringComparison.OrdinalIgnoreCase))
                {
                    return InputFormatterResult.SuccessAsync(null);
                }
            }

            var model = _typeModel.Deserialize(request.Body, null, context.ModelType);
            return InputFormatterResult.SuccessAsync(model);
        }

        protected override bool CanReadType(Type type)
        {
            return _typeModel.CanSerialize(type);
        }
    }
}