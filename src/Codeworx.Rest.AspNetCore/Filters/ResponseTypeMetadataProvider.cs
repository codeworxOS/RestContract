using System.Linq;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace Codeworx.Rest.AspNetCore.Filters
{
    public class ResponseTypeMetadataProvider : AttributeMetadataProvider<ResponseTypeAttribute>
    {
        protected override void TransformAction(ResponseTypeAttribute source, ActionModel model, MetadataProviderContext context)
        {
            model.Selectors.First().EndpointMetadata.Add(source);
        }

        protected override void TransformController(ResponseTypeAttribute source, ControllerModel model, MetadataProviderContext context)
        {
            model.Selectors.First().EndpointMetadata.Add(source);
        }

        protected override void TransformParameter(ResponseTypeAttribute source, ParameterModel model, MetadataProviderContext context)
        {
            // do nothing
        }
    }
}
