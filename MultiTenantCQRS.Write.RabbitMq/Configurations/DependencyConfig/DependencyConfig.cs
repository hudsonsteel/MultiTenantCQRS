using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MultiTenantCQRS.Write.Domain.Interfaces.Bus;
using MultiTenantCQRS.Write.RabbitMq.Bus;
using MultiTenantCQRS.Write.RabbitMq.Options;

namespace MultiTenantCQRS.Write.RabbitMq.Configurations.DependencyConfig
{
    public static class DependencyConfig
    {
        public static void AddRabbitMqConfig(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<RabbitMqOption>(configuration.GetSection("TenantProviders"));

            services.AddScoped<IEventBus, RabbitMqEventBus>();

            services.AddRabbitMqConfiguration(configuration);
        }
    }
}
