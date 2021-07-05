using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace Codeworx.Rest.AspNetCore
{
    public abstract class AttributeMetadataProvider<TSource> : IAttributeMetadataProvider
    {
        public void ProcessAction(IEnumerable<object> attributes, ActionModel action, MetadataProviderContext context)
        {
            foreach (var item in attributes.OfType<TSource>())
            {
                TransformAction(item, action, context);
            }
        }

        public void ProcessController(IEnumerable<object> attributes, ControllerModel controller, MetadataProviderContext context)
        {
            foreach (var item in attributes.OfType<TSource>())
            {
                TransformController(item, controller, context);
            }
        }

        public void ProcessParameter(IEnumerable<object> attributes, ParameterModel parameter, MetadataProviderContext context)
        {
            foreach (var item in attributes.OfType<TSource>())
            {
                TransformParameter(item, parameter, context);
            }
        }

        protected abstract void TransformAction(TSource source, ActionModel model, MetadataProviderContext context);

        protected abstract void TransformController(TSource source, ControllerModel model, MetadataProviderContext context);

        protected abstract void TransformParameter(TSource source, ParameterModel model, MetadataProviderContext context);
    }
}
