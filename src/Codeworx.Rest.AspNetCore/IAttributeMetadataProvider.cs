using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace Codeworx.Rest.AspNetCore
{
    public interface IAttributeMetadataProvider
    {
        void ProcessController(IEnumerable<object> attributes, ControllerModel controller, MetadataProviderContext context);

        void ProcessAction(IEnumerable<object> attributes, ActionModel action, MetadataProviderContext context);

        void ProcessParameter(IEnumerable<object> attributes, ParameterModel parameter, MetadataProviderContext context);
    }
}
