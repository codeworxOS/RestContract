using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace Codeworx.Rest.AspNetCore
{
    public interface IAttributeMetadataProvider
    {
        void ProcessController(IEnumerable<object> attributes, ControllerModel controller);

        void ProcessAction(IEnumerable<object> attributes, ActionModel action);

        void ProcessParameter(IEnumerable<object> attributes, ParameterModel parameter);
    }
}
