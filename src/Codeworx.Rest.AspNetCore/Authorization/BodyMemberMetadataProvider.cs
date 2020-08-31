using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Codeworx.Rest.AspNetCore.Authorization
{
    public class BodyMemberMetadataProvider : AttributeMetadataProvider<BodyMemberAttribute>
    {
        protected override void TransformAction(BodyMemberAttribute source, ActionModel model)
        {
            // do nothing
        }

        protected override void TransformController(BodyMemberAttribute source, ControllerModel model)
        {
            // do nothing;
        }

        protected override void TransformParameter(BodyMemberAttribute source, ParameterModel model)
        {
            model.BindingInfo = BindingInfo.GetBindingInfo(new object[] { new FromBodyAttribute() });
        }
    }
}
