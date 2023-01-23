using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace Codeworx.Rest.AspNetCore.Filters
{
    public class ResponseTypeMetadataProvider : AttributeMetadataProvider<ResponseTypeAttribute>
    {
        protected override void TransformAction(ResponseTypeAttribute source, ActionModel model, MetadataProviderContext context)
        {
            model.Filters.Add(GetTarget(source));
        }

        protected override void TransformController(ResponseTypeAttribute source, ControllerModel model, MetadataProviderContext context)
        {
            model.Filters.Add(GetTarget(source));
        }

        protected override void TransformParameter(ResponseTypeAttribute source, ParameterModel model, MetadataProviderContext context)
        {
            // do nothing
        }

        private static ProducesResponseTypeAttribute GetTarget(ResponseTypeAttribute source)
        {
            if (source.PayloadType == null)
            {
                return new ProducesResponseTypeAttribute(source.StatusCode);
            }

            return new ProducesResponseTypeAttribute(source.PayloadType, source.StatusCode);
        }
    }
}
