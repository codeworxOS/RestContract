using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace Codeworx.Rest.AspNetCore
{
    public class ProducesMetadataProvider : IAttributeMetadataProvider
    {
        public void ProcessAction(IEnumerable<object> attributes, ActionModel action, MetadataProviderContext context)
        {
            var contentTypes = attributes.OfType<ResponseTypeAttribute>().Where(p => p.ContentType != null).OrderBy(p => p.StatusCode == 200 ? 0 : 1).Select(p => p.ContentType).ToList();

            if (contentTypes.Any())
            {
                action.Filters.Add(new ProducesAttribute(contentTypes[0], contentTypes.Skip(1).ToArray()));
            }
        }

        public void ProcessController(IEnumerable<object> attributes, ControllerModel controller, MetadataProviderContext context)
        {
            return;
        }

        public void ProcessParameter(IEnumerable<object> attributes, ParameterModel parameter, MetadataProviderContext context)
        {
            return;
        }
    }
}
