using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MultiTenantCQRS.Contract.Event.Events.CustomerOrders;
using MultiTenantCQRS.Write.RabbitMq.Options;

namespace MultiTenantCQRS.Write.RabbitMq.Configurations.DependencyConfig
{
    public static class RabbitMqConfigurationExtension
    {
        public static void AddRabbitMqConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            var rabbitMqOptions = configuration.GetSection("RabbitMQ").Get<RabbitMqOption>();

            services.AddSingleton(rabbitMqOptions);

            services.AddMassTransit(cfg =>
            {
                cfg.AddBus(provider => CreateBus(provider, rabbitMqOptions));
            });
        }

        private static IBusControl CreateBus(IBusRegistrationContext provider, RabbitMqOption config)
        {
            return MassTransit.Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                cfg.Host(new Uri($"rabbitmq://{config.Host}:{config.Port}/{config.VirtualHost}"), h =>
                {
                    h.Username(config.UserName);
                    h.Password(config.Password);
                });

                ConfigurePublishers(cfg);

                cfg.ConfigureEndpoints(provider);
            });
        }

        private static void ConfigurePublishers(IRabbitMqBusFactoryConfigurator cfg)
        {
            ConfigurePublisher<CustomerOrdersCreatedOrUpdateEvent>(cfg);
            ConfigurePublisher<CustomerOrdersDeletedEvent>(cfg);
        }

        private static void ConfigurePublisher<T>(IRabbitMqBusFactoryConfigurator cfg) where T : class
        {
            cfg.Publish<T>(x =>
            {
                x.ExchangeType = "fanout";
            });
        }
    }
}
