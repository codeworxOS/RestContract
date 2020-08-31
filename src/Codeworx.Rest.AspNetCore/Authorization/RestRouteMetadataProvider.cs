using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace Codeworx.Rest.AspNetCore.Authorization
{
    public class RestRouteMetadataProvider : AttributeMetadataProvider<RestRouteAttribute>
    {
        protected override void TransformAction(RestRouteAttribute source, ActionModel model)
        {
            // do nothing;
        }

        protected override void TransformController(RestRouteAttribute source, ControllerModel model)
        {
            model.Selectors.First().AttributeRouteModel = new AttributeRouteModel(new RouteAttribute(source.RoutePrefix));
        }

        protected override void TransformParameter(RestRouteAttribute source, ParameterModel model)
        {
            // do nothing;
        }
    }
}
