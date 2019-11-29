using System;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
#if NETSTANDARD2_0
using Microsoft.AspNetCore.Mvc.Internal;
#endif
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Routing;

namespace Codeworx.Rest.AspNetCore
{
    public class ContractModelProvider : IApplicationModelProvider
    {
        public int Order => 9999;

        public void OnProvidersExecuted(ApplicationModelProviderContext context)
        {
        }

        public void OnProvidersExecuting(ApplicationModelProviderContext context)
        {
            foreach (var controller in context.Result.Controllers)
            {
                var serviceInterfaces = controller.ControllerType.GetInterfaces();
                var serviceInterface = serviceInterfaces.FirstOrDefault(p => p.GetCustomAttribute<RestRouteAttribute>() != null);

                var att = controller.ControllerType.GetCustomAttribute<RestRouteAttribute>();
                att = att ?? serviceInterface?.GetTypeInfo()?.GetCustomAttribute<RestRouteAttribute>();

                if (att != null)
                {
                    controller.Selectors.First().AttributeRouteModel = new AttributeRouteModel(new RouteAttribute(att.RoutePrefix));
                }

                foreach (var action in controller.Actions)
                {
                    var method = action.ActionMethod;

                    var methodAtt = method.GetCustomAttributes().OfType<RestOperationAttribute>().FirstOrDefault();

                    if (methodAtt == null)
                    {
                        method = serviceInterfaces
                                    .Select(p => p.GetTypeInfo().GetMethod(method.Name, method.GetParameters().Select(x => x.ParameterType).ToArray()))
                                    .FirstOrDefault(p => p != null);

                        methodAtt = method?.GetCustomAttributes().OfType<RestOperationAttribute>().FirstOrDefault();
                    }

                    if (methodAtt != null)
                    {
                        var targetAttribute = GetTargetAttribute(methodAtt.HttpMethod(), methodAtt.Template);

                        action.Selectors.Clear();
                        action.Selectors.Add(CreateSelectorModel(targetAttribute, action.ActionMethod));

                        for (int i = 0; i < action.Parameters.Count; i++)
                        {
                            if (method.GetParameters()[i].GetCustomAttribute<BodyMemberAttribute>() != null)
                            {
                                action.Parameters[i].BindingInfo = BindingInfo.GetBindingInfo(new object[] { new FromBodyAttribute() });
                            }
                        }
                    }
                }
            }
        }

        private static SelectorModel CreateSelectorModel(IRouteTemplateProvider route, MethodInfo method)
        {
            var selectorModel = new SelectorModel();
            if (route != null)
            {
                selectorModel.AttributeRouteModel = new AttributeRouteModel(route);
            }

            foreach (var item in method.GetCustomAttributes().OfType<IActionConstraintMetadata>())
            {
                selectorModel.ActionConstraints.Add(item);
                selectorModel.EndpointMetadata.Add(item);
            }

            selectorModel.EndpointMetadata.Add(route);

            // Simple case, all HTTP method attributes apply
            var httpMethods = new[] { route }
                .OfType<IActionHttpMethodProvider>()
                .SelectMany(a => a.HttpMethods)
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToArray();

            if (httpMethods.Length > 0)
            {
                selectorModel.ActionConstraints.Add(new HttpMethodActionConstraint(httpMethods));
                selectorModel.EndpointMetadata.Add(new HttpMethodMetadata(httpMethods));
            }

            return selectorModel;
        }

        private IRouteTemplateProvider GetTargetAttribute(string method, string template)
        {
            switch (method)
            {
                case "GET":
                    return template == null ? new HttpGetAttribute() : new HttpGetAttribute(template);

                case "POST":
                    return template == null ? new HttpPostAttribute() : new HttpPostAttribute(template);

                case "PUT":
                    return template == null ? new HttpPutAttribute() : new HttpPutAttribute(template);

                case "DELETE":
                    return template == null ? new HttpDeleteAttribute() : new HttpDeleteAttribute(template);

                case "HEAD":
                    return template == null ? new HttpHeadAttribute() : new HttpHeadAttribute(template);

                case "OPTIONS":
                    return template == null ? new HttpOptionsAttribute() : new HttpOptionsAttribute(template);

                case "PATCH":
                    return template == null ? new HttpPatchAttribute() : new HttpPatchAttribute(template);

                default:
                    throw new NotSupportedException($"Http method {method} is not supported by the rest contract package!");
            }
        }
    }
}