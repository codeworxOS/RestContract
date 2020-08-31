using System.Linq;
using Codeworx.Rest.Authorization;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.Authorization;

namespace Codeworx.Rest.AspNetCore.Authorization
{
    public class PolicyMetadataProvider : AttributeMetadataProvider<PolicyAttribute>
    {
        protected override void TransformAction(PolicyAttribute source, ActionModel model)
        {
            model.Filters.Add(new AuthorizeFilter(source.Name));
            model.Selectors.First().EndpointMetadata.Add(new AuthorizeFilter(source.Name));
        }

        protected override void TransformController(PolicyAttribute source, ControllerModel model)
        {
            model.Filters.Add(new AuthorizeFilter(source.Name));
            model.Selectors.First().EndpointMetadata.Add(new AuthorizeFilter(source.Name));
        }

        protected override void TransformParameter(PolicyAttribute source, ParameterModel model)
        {
            // do nothing;
        }
    }
}
