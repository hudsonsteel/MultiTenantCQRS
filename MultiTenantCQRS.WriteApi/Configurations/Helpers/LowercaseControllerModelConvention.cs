using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace MultiTenantCQRS.WriteApi.Configurations.Helpers
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
        }

        private static string ToLowerCaseRoute(string route)
        {
            return string.Join('/', route.Split('/').Select(x => x.ToLower()));
        }
    }
}
