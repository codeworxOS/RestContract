using System;
using Codeworx.Rest;
using Codeworx.Rest.Formatters.Protobuf;
using ProtoBuf.Meta;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class CodeworxRestFormattersProtobufRestOptionsBuilderExtensions
    {
        public static IRestOptionsBuilder AddProtobufFormatter(this IRestOptionsBuilder builder, Func<TypeModel> model = null, bool makeDefault = false)
        {
            builder.Services.AddOrReplace<IContentFormatter, ProtobufContentFormatter>(ServiceLifetime.Singleton, sp =>
            {
                return new ProtobufContentFormatter(model?.Invoke() ?? RuntimeTypeModel.Default);
            });

            if (makeDefault)
            {
                builder.Services.AddOrReplace<DefaultFormatterSelector>(ServiceLifetime.Scoped, sp => () => "application/x-protobuf");
            }

            return builder;
        }
    }
}