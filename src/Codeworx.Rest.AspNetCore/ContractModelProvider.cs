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
                var metadataContext = new MetadataProviderContext();

                var serviceInterfaces = controller.ControllerType.GetInterfaces();
                var attributes = serviceInterfaces.SelectMany(p => p.GetCustomAttributes()).ToList();
                foreach (var provider in _metadataProviders)
                {
                    provider.ProcessController(attributes, controller, metadataContext);
                }

                if (metadataContext.CanProcess)
                {
                    foreach (var action in controller.Actions)
                    {
                        var method = action.ActionMethod;

                        method = serviceInterfaces
                                    .Select(p => p.GetTypeInfo().GetMethod(method.Name, method.GetParameters().Select(x => x.ParameterType).ToArray()))
                                    .FirstOrDefault(p => p != null);

                        if (method == null)
                        {
                            metadataContext.ActionsToRemove.Add(action);
                            continue;
                        }

                        metadataContext.CurrentAction = method;

                        var actionAttributes = method.GetCustomAttributes().ToList();
                        foreach (var item in _metadataProviders)
                        {
                            item.ProcessAction(actionAttributes, action, metadataContext);
                        }

                        for (int i = 0; i < action.Parameters.Count; i++)
                        {
                            var parameterAttributes = method.GetParameters()[i].GetCustomAttributes();

                            foreach (var item in _metadataProviders)
                            {
                                item.ProcessParameter(parameterAttributes, action.Parameters[i], metadataContext);
                            }
                        }
                    }

                    foreach (var action in metadataContext.ActionsToRemove)
                    {
                        controller.Actions.Remove(action);
                    }
                }
            }
        }
    }
}