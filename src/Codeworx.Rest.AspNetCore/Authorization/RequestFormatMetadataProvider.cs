using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace Codeworx.Rest.AspNetCore.Authorization
{
    public class RequestFormatMetadataProvider : AttributeMetadataProvider<RequestFormatAttribute>
    {
        protected override void TransformAction(RequestFormatAttribute source, ActionModel model, MetadataProviderContext context)
        {
            var consumesAttribute = new ConsumesAttribute(source.ContentType, source.AdditionalContentTypes.ToArray());

            model.Filters.Add(consumesAttribute);

            var selectorModel = model.Selectors.First();
            selectorModel.ActionConstraints.Add(consumesAttribute);
            selectorModel.EndpointMetadata.Add(consumesAttribute);
        }

        protected override void TransformController(RequestFormatAttribute source, ControllerModel model, MetadataProviderContext context)
        {
            var consumesAttribute = new ConsumesAttribute(source.ContentType, source.AdditionalContentTypes.ToArray());

            model.Filters.Add(consumesAttribute);

            var selectorModel = model.Selectors.First();
            selectorModel.ActionConstraints.Add(consumesAttribute);
            selectorModel.EndpointMetadata.Add(consumesAttribute);
        }

        protected override void TransformParameter(RequestFormatAttribute source, ParameterModel model, MetadataProviderContext context)
        {
            // do nothing
        }
    }
}
