using System.Linq;
using Codeworx.Rest.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.Authorization;

namespace Codeworx.Rest.AspNetCore.Authorization
{
    public class AnonymousMetadataProvider : AttributeMetadataProvider<AnonymousAttribute>
    {
        protected override void TransformAction(AnonymousAttribute source, ActionModel model, MetadataProviderContext context)
        {
            model.Filters.Add(new AllowAnonymousFilter());
            model.Selectors.First().EndpointMetadata.Add(new AllowAnonymousAttribute());
        }

        protected override void TransformController(AnonymousAttribute source, ControllerModel model, MetadataProviderContext context)
        {
            model.Filters.Add(new AllowAnonymousFilter());
            model.Selectors.First().EndpointMetadata.Add(new AllowAnonymousAttribute());
        }

        protected override void TransformParameter(AnonymousAttribute source, ParameterModel model, MetadataProviderContext context)
        {
            // do nothing;
        }
    }
}
