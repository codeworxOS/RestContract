using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace Codeworx.Rest.AspNetCore
{
    public class ContractModelProvider : IApplicationModelProvider
    {
        private readonly IEnumerable<IAttributeMetadataProvider> _metadataProviders;

        public ContractModelProvider(IEnumerable<IAttributeMetadataProvider> metadataProviders)
        {
            _metadataProviders = metadataProviders;
        }

        public int Order => 9999;

        public void OnProvidersExecuted(ApplicationModelProviderContext context)
        {
        }

        public void OnProvidersExecuting(ApplicationModelProviderContext context)
        {
            foreach (var controller in context.Result.Controllers)
            {
                var serviceInterfaces = controller.ControllerType.GetInterfaces();
                var attributes = serviceInterfaces.SelectMany(p => p.GetCustomAttributes()).ToList();
                foreach (var provider in _metadataProviders)
                {
                    provider.ProcessController(attributes, controller);
                }

                var actionsToRemove = new List<ActionModel>();
                foreach (var action in controller.Actions)
                {
                    var method = action.ActionMethod;

                    method = serviceInterfaces
                                .Select(p => p.GetTypeInfo().GetMethod(method.Name, method.GetParameters().Select(x => x.ParameterType).ToArray()))
                                .FirstOrDefault(p => p != null);

                    if (method == null)
                    {
                        actionsToRemove.Add(action);
                        continue;
                    }

                    var actionAttributes = method.GetCustomAttributes().ToList();
                    foreach (var item in _metadataProviders)
                    {
                        item.ProcessAction(actionAttributes, action);
                    }

                    for (int i = 0; i < action.Parameters.Count; i++)
                    {
                        var parameterAttributes = method.GetParameters()[i].GetCustomAttributes();

                        foreach (var item in _metadataProviders)
                        {
                            item.ProcessParameter(parameterAttributes, action.Parameters[i]);
                        }
                    }

                    ////if (routeAttribute != null)
                    ////{
                    ////    actionsToRemove.Add(action);
                    ////}
                }

                foreach (var action in actionsToRemove)
                {
                    controller.Actions.Remove(action);
                }
            }
        }
    }
}