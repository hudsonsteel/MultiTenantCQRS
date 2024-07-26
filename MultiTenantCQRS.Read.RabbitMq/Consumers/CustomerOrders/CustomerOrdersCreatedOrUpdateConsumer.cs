using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using MultiTenantCQRS.Contract.Event.Events.CustomerOrders;
using MultiTenantCQRS.Read.Application.Commands.CustomerOrders;

namespace MultiTenantCQRS.Read.RabbitMq.Consumers.CustomerOrders
{
    public class CustomerOrdersCreatedOrUpdateConsumer(
        ILogger<CustomerOrdersCreatedOrUpdateConsumer> logger,
        IMediator mediator) : IConsumer<CustomerOrdersCreatedOrUpdateEvent>
    {
        private readonly ILogger<CustomerOrdersCreatedOrUpdateConsumer> _logger = logger;
        private readonly IMediator _mediator = mediator;

        public async Task Consume(ConsumeContext<CustomerOrdersCreatedOrUpdateEvent> context)
        {
            var orderCreatedEvent = context.Message;

            _logger.LogInformation($"Started processing CustomerOrders: OrderId={orderCreatedEvent.Id}, TenantId={orderCreatedEvent.TenantId}");

            try
            {
                var command = CreateCommandFromEvent(orderCreatedEvent);
                await _mediator.Send(command);

                _logger.LogInformation($"Finished processing CustomerOrders: OrderId={orderCreatedEvent.Id}, TenantId={orderCreatedEvent.TenantId}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error processing CustomerOrders: OrderId={orderCreatedEvent.Id}, TenantId={orderCreatedEvent.TenantId}");
                throw;
            }
        }

        private static CreateOrUpdateCustomerOrdersCommand CreateCommandFromEvent(CustomerOrdersCreatedOrUpdateEvent orderCreatedEvent)
        {
            return new CreateOrUpdateCustomerOrdersCommand
            {
                TenantId = orderCreatedEvent.TenantId,
                Id = orderCreatedEvent.Id,
                Email = orderCreatedEvent.Email,
                Name = orderCreatedEvent.Name,
                Orders = orderCreatedEvent.Orders.Select(orderEvent => new Domain.Dtos.Order
                (
                    orderEvent.Id,
                    orderEvent.Date,
                    orderEvent.TotalAmount
                )).ToList()
            };
        }
    }
}
