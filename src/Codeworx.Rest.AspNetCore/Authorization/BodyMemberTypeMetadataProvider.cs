using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace Codeworx.Rest.AspNetCore.Authorization
{
    public class BodyMemberTypeMetadataProvider : AttributeMetadataProvider<BodyMemberTypeAttribute>
    {
        protected override void TransformAction(BodyMemberTypeAttribute source, ActionModel model, MetadataProviderContext context)
        {
            var consumesAttribute = new ConsumesAttribute(source.ContentType, source.AdditionalContentTypes.ToArray());

            model.Filters.Add(consumesAttribute);

            var selectorModel = model.Selectors.First();
            selectorModel.ActionConstraints.Add(consumesAttribute);
            selectorModel.EndpointMetadata.Add(consumesAttribute);
        }

        protected override void TransformController(BodyMemberTypeAttribute source, ControllerModel model, MetadataProviderContext context)
        {
            // do nothing
        }

        protected override void TransformParameter(BodyMemberTypeAttribute source, ParameterModel model, MetadataProviderContext context)
        {
            // do nothing
        }
    }
}
