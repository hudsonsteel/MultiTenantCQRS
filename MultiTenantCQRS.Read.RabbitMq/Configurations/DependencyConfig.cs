using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MultiTenantCQRS.Read.RabbitMq.Options;

namespace MultiTenantCQRS.Read.RabbitMq.Configurations
{
    public static class DependencyConfig
    {
        public static void AddRabbitMqConfig(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<RabbitMqOption>(configuration.GetSection("RabbitMQ"));

            services.AddSingleton(sp => sp.GetRequiredService<IOptions<RabbitMqOption>>().Value);

            services.AddRabbitMqConfiguration();
        }
    }
}
