using System.Linq;
using Codeworx.Rest.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.Authorization;

namespace Codeworx.Rest.AspNetCore.Authorization
{
    public class AnonymousMetadataProvider : AttributeMetadataProvider<AnonymousAttribute>
    {
        protected override void TransformAction(AnonymousAttribute source, ActionModel model)
        {
            model.Filters.Add(new AllowAnonymousFilter());
            model.Selectors.First().EndpointMetadata.Add(new AllowAnonymousAttribute());
        }

        protected override void TransformController(AnonymousAttribute source, ControllerModel model)
        {
            model.Filters.Add(new AllowAnonymousFilter());
            model.Selectors.First().EndpointMetadata.Add(new AllowAnonymousAttribute());
        }

        protected override void TransformParameter(AnonymousAttribute source, ParameterModel model)
        {
            // do nothing;
        }
    }
}
