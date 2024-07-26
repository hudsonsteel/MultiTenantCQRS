using Microsoft.Extensions.DependencyInjection;
using MultiTenantCQRS.Write.Application.CommandHandlers.Customer;
using MultiTenantCQRS.Write.Application.CommandHandlers.Order;
using MultiTenantCQRS.Write.Domain.Interfaces.Bus;
using MultiTenantCQRS.Write.RabbitMq.Bus;

namespace MultiTenantCQRS.Write.Application.Configurations
{
    public static class DependencyConfig
    {
        public static void AddAplicationConfig(this IServiceCollection services)
        {
            services.AddScoped<IEventBus, RabbitMqEventBus>();

            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CreateCustomerCommandHandler).Assembly));
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(DeleteCustomerCommandHandler).Assembly));
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(UpdateCustomerCommandHandler).Assembly));
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CreateOrderCommandHandler).Assembly));
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(DeleteOrderCommandHandler).Assembly));
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(UpdateOrderCommandHandler).Assembly));
        }
    }
}
