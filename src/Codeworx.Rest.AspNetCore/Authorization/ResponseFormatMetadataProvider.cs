using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace Codeworx.Rest.AspNetCore.Authorization
{
    public class ResponseFormatMetadataProvider : AttributeMetadataProvider<ResponseFormatAttribute>
    {
        protected override void TransformAction(ResponseFormatAttribute source, ActionModel model, MetadataProviderContext context)
        {
            var producesAttribute = new ProducesAttribute(source.ContentType, source.AdditionalContentTypes.ToArray());

            model.Filters.Add(producesAttribute);

            var selectorModel = model.Selectors.First();
            selectorModel.EndpointMetadata.Add(producesAttribute);
        }

        protected override void TransformController(ResponseFormatAttribute source, ControllerModel model, MetadataProviderContext context)
        {
            var producesAttribute = new ProducesAttribute(source.ContentType, source.AdditionalContentTypes.ToArray());

            model.Filters.Add(producesAttribute);

            var selectorModel = model.Selectors.First();
            selectorModel.EndpointMetadata.Add(producesAttribute);
        }

        protected override void TransformParameter(ResponseFormatAttribute source, ParameterModel model, MetadataProviderContext context)
        {
            // do nothing
        }
    }
}
