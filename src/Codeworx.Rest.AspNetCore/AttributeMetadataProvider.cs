using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace Codeworx.Rest.AspNetCore
{
    public abstract class AttributeMetadataProvider<TSource> : IAttributeMetadataProvider
    {
        public void ProcessAction(IEnumerable<object> attributes, ActionModel action)
        {
            foreach (var item in attributes.OfType<TSource>())
            {
                TransformAction(item, action);
            }
        }

        public void ProcessController(IEnumerable<object> attributes, ControllerModel controller)
        {
            foreach (var item in attributes.OfType<TSource>())
            {
                TransformController(item, controller);
            }
        }

        public void ProcessParameter(IEnumerable<object> attributes, ParameterModel parameter)
        {
            foreach (var item in attributes.OfType<TSource>())
            {
                TransformParameter(item, parameter);
            }
        }

        protected abstract void TransformAction(TSource source, ActionModel model);

        protected abstract void TransformController(TSource source, ControllerModel model);

        protected abstract void TransformParameter(TSource source, ParameterModel model);
    }
}
