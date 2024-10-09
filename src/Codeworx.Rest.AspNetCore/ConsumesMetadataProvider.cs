using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace Codeworx.Rest.AspNetCore
{
    public class ConsumesMetadataProvider : IAttributeMetadataProvider
    {
        public void ProcessAction(IEnumerable<object> attributes, ActionModel action, MetadataProviderContext context)
        {
            if (context.CurrentAction != null)
            {
                var contentTypes = context.CurrentAction.GetParameters()
                                    .SelectMany(p => p.GetCustomAttributes<BodyMemberAttribute>())
                                    .SelectMany(p => p.ContentTypes)
                                    .ToList();

                if (contentTypes.Any())
                {
                    action.Filters.Add(new ConsumesAttribute(contentTypes.First(), contentTypes.Skip(1).ToArray()));
                }
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
