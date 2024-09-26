using System;
using Codeworx.Rest;
using Codeworx.Rest.Formatters.Protobuf;
using ProtoBuf.Meta;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class CodeworxRestFormattersProtobufRestOptionsBuilderExtensions
    {
        private static object _typeModelLocker = new object();
        private static RuntimeTypeModel _typeModel = null;

        public static IRestOptionsBuilder AddProtobufFormatter(this IRestOptionsBuilder builder, Func<TypeModel> model = null, bool makeDefault = false)
        {
            builder.Services.AddOrReplace<IContentFormatter, ProtobufContentFormatter>(ServiceLifetime.Singleton, sp =>
            {
                return new ProtobufContentFormatter(model?.Invoke() ?? CreateDefaultModel());
            });

            if (makeDefault)
            {
                builder.Services.AddOrReplace<DefaultFormatterSelector>(ServiceLifetime.Scoped, sp => () => "application/x-protobuf");
            }

            return builder;
        }

        private static TypeModel CreateDefaultModel()
        {
            if (_typeModel != null)
            {
                return _typeModel;
            }

            lock (_typeModelLocker)
            {
                if (_typeModel == null)
                {
                    _typeModel = RuntimeTypeModel.Default;
                    _typeModel.SetSurrogate<DateTimeOffset, DateTimeOffsetSurrogate>();
                }

                return _typeModel;
            }
        }
    }
}