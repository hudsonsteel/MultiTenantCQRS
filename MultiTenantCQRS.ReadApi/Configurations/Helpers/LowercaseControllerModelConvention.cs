using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace MultiTenantCQRS.ReadApi.Configurations.Helpers
{

    public class LowercaseControllerModelConvention : IControllerModelConvention
    {
        public void Apply(ControllerModel controller)
        {
            foreach (var selector in controller.Selectors)
            {
                if (selector.AttributeRouteModel != null)
                {
                    selector.AttributeRouteModel.Template = ToLowerCaseRoute(selector.AttributeRouteModel.Template);
                }
            }

            foreach (var action in controller.Actions)
            {
                foreach (var selector in action.Selectors)
                {
                    if (selector.AttributeRouteModel != null)
                    {
                        selector.AttributeRouteModel.Template = ToLowerCaseRoute(selector.AttributeRouteModel.Template);
                    }
                }
            }
        }

        private string ToLowerCaseRoute(string route)
        {
            return string.Join('/', route.Split('/').Select(x => x.ToLower()));
        }
    }
}
