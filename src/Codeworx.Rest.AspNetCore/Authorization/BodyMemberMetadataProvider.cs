using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Codeworx.Rest.AspNetCore.Authorization
{
    public class BodyMemberMetadataProvider : AttributeMetadataProvider<BodyMemberAttribute>
    {
        protected override void TransformAction(BodyMemberAttribute source, ActionModel model, MetadataProviderContext context)
        {
            // do nothing
        }

        protected override void TransformController(BodyMemberAttribute source, ControllerModel model, MetadataProviderContext context)
        {
            // do nothing;
        }

        protected override void TransformParameter(BodyMemberAttribute source, ParameterModel model, MetadataProviderContext context)
        {
            model.BindingInfo = BindingInfo.GetBindingInfo(new object[] { new FromBodyAttribute() });
        }
    }
}
