using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MultiTenantCQRS.Contract.Event.Events.CustomerOrders;
using MultiTenantCQRS.Read.RabbitMq.Consumers.CustomerOrders;
using MultiTenantCQRS.Read.RabbitMq.Options;

namespace MultiTenantCQRS.Read.RabbitMq.Configurations
{
    public static class RabbitMqConfigurationExtension
    {
        public static void AddRabbitMqConfiguration(this IServiceCollection services)
        {
            services.AddMassTransit(x =>
            {
                x.AddConsumer<CustomerOrdersCreatedOrUpdateConsumer>();
                x.AddConsumer<CustomerOrdersDeletedConsumer>();

                x.UsingRabbitMq((context, cfg) =>
                {
                    var rabbitMqConfig = context.GetRequiredService<RabbitMqOption>();

                    cfg.Host(new Uri($"rabbitmq://{rabbitMqConfig.Host}:{rabbitMqConfig.Port}/{rabbitMqConfig.VirtualHost}"), h =>
                    {
                        h.Username(rabbitMqConfig.UserName);
                        h.Password(rabbitMqConfig.Password);
                    });

                    ConfigureReceiveEndpoints(cfg, rabbitMqConfig, context);

                    cfg.ConfigureEndpoints(context);
                });
            });
        }

        private static void ConfigureReceiveEndpoints(IRabbitMqBusFactoryConfigurator cfg, RabbitMqOption rabbitMqConfig, IBusRegistrationContext context)
        {
            var endpoints = new[]
            {
            nameof(CustomerOrdersCreatedOrUpdateEvent),
            nameof(CustomerOrdersDeletedEvent)
            };

            foreach (var endpointName in endpoints)
            {
                cfg.ReceiveEndpoint(endpointName, e =>
                {
                    e.ConfigureConsumeTopology = true;
                    e.Bind(rabbitMqConfig.ExchangeName, x =>
                    {
                        x.ExchangeType = "fanout";
                        x.Durable = true;
                    });

                    var consumerType = endpointName switch
                    {
                        nameof(CustomerOrdersCreatedOrUpdateEvent) => typeof(CustomerOrdersCreatedOrUpdateConsumer),
                        nameof(CustomerOrdersDeletedEvent) => typeof(CustomerOrdersDeletedConsumer),
                        _ => throw new ArgumentException($"No consumer found for endpoint {endpointName}")
                    };

                    e.Consumer(consumerType, context.GetService);
                });
            }
        }
    }

}
