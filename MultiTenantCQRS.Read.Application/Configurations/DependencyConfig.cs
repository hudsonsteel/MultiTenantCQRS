using Microsoft.Extensions.DependencyInjection;
using MultiTenantCQRS.Read.Application.CommandHandlers.CustomerOrders;

namespace MultiTenantCQRS.Read.Application.Configurations
{
    public static class DependencyConfig
    {
        public static void AddAplicationConfig(this IServiceCollection services)
        {
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CreateOrUpdateCustomerOrdersCommandHandler).Assembly));
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(DeleteCustomerOrdersCommandHandler).Assembly));
        }
    }
}
