using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace Codeworx.Rest.AspNetCore
{
    public class IgnoreApiMetadataProvider : AttributeMetadataProvider<IgnoreApiAttribute>
    {
        protected override void TransformAction(IgnoreApiAttribute source, ActionModel model, MetadataProviderContext context)
        {
            model.ApiExplorer.IsVisible = false;
        }

        protected override void TransformController(IgnoreApiAttribute source, ControllerModel model, MetadataProviderContext context)
        {
            model.ApiExplorer.IsVisible = false;
        }

        protected override void TransformParameter(IgnoreApiAttribute source, ParameterModel model, MetadataProviderContext context)
        {
            return;
        }
    }
}
