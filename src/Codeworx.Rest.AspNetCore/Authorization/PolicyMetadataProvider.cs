using System.Linq;
using Codeworx.Rest.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
#if NETSTANDARD
using Microsoft.AspNetCore.Mvc.Authorization;
#endif

namespace Codeworx.Rest.AspNetCore.Authorization
{
    public class PolicyMetadataProvider : AttributeMetadataProvider<PolicyAttribute>
    {
        protected override void TransformAction(PolicyAttribute source, ActionModel model)
        {
#if NETSTANDARD
            model.Filters.Add(new AuthorizeFilter(source.Name));
#endif
            model.Selectors.First().EndpointMetadata.Add(new AuthorizeAttribute(source.Name));
        }

        protected override void TransformController(PolicyAttribute source, ControllerModel model)
        {
#if NETSTANDARD
            model.Filters.Add(new AuthorizeFilter(source.Name));
#endif
            model.Selectors.First().EndpointMetadata.Add(new AuthorizeAttribute(source.Name));
        }

        protected override void TransformParameter(PolicyAttribute source, ParameterModel model)
        {
            // do nothing;
        }
    }
}
