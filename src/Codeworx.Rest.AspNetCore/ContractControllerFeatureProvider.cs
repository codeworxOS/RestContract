using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Controllers;

namespace Codeworx.Rest.AspNetCore
{
    public class ContractControllerFeatureProvider : IApplicationFeatureProvider<ControllerFeature>
    {
        public void PopulateFeature(IEnumerable<ApplicationPart> parts, ControllerFeature feature)
        {
            foreach (var item in parts.OfType<AssemblyPart>())
            {
                foreach (var controllerType in item.Types.Where(p => IsController(p)))
                {
                    if (!feature.Controllers.Contains(controllerType))
                    {
                        feature.Controllers.Add(controllerType);
                    }
                }
            }
        }

        private bool IsController(TypeInfo type)
        {
            var hasContract = type.IsClass &&
                !type.IsAbstract &&
                type.GetInterfaces().Any(p => p.GetCustomAttribute<RestRouteAttribute>() != null);

            if (!hasContract)
            {
                return false;
            }

            do
            {
                if (type.Name.StartsWith("RestClient"))
                {
                    return false;
                }

                type = type.BaseType?.GetTypeInfo();
            }
            while (type != null);

            return true;
        }
    }
}