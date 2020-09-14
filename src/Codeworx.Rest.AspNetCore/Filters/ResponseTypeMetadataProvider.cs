using System.Linq;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace Codeworx.Rest.AspNetCore.Filters
{
    public class ResponseTypeMetadataProvider : AttributeMetadataProvider<ResponseTypeAttribute>
    {
        protected override void TransformAction(ResponseTypeAttribute source, ActionModel model)
        {
            model.Selectors.First().EndpointMetadata.Add(source);
        }

        protected override void TransformController(ResponseTypeAttribute source, ControllerModel model)
        {
            model.Selectors.First().EndpointMetadata.Add(source);
        }

        protected override void TransformParameter(ResponseTypeAttribute source, ParameterModel model)
        {
            // do nothing
        }
    }
}
