using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using MultiTenantCQRS.Contract.Event.Events.CustomerOrders;
using MultiTenantCQRS.Read.Application.Commands.CustomerOrders;

namespace MultiTenantCQRS.Read.RabbitMq.Consumers.CustomerOrders
{
    public class CustomerOrdersDeletedConsumer(
        ILogger<CustomerOrdersDeletedConsumer> logger,
        IMediator mediator) : IConsumer<CustomerOrdersDeletedEvent>
    {
        private readonly ILogger<CustomerOrdersDeletedConsumer> _logger = logger;
        private readonly IMediator _mediator = mediator;

        public async Task Consume(ConsumeContext<CustomerOrdersDeletedEvent> context)
        {
            var customerOrdersDeletedEvent = context.Message;

            _logger.LogInformation($"Started processing CustomerOrders deletion: OrderId={customerOrdersDeletedEvent.Id}, TenantId={customerOrdersDeletedEvent.TenantId}");

            try
            {
                var command = CreateCommandFromEvent(customerOrdersDeletedEvent);
                await _mediator.Send(command);
                _logger.LogInformation($"Successfully finished processing CustomerOrders deletion: OrderId={customerOrdersDeletedEvent.Id}, TenantId={customerOrdersDeletedEvent.TenantId}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error processing CustomerOrders deletion: OrderId={customerOrdersDeletedEvent.Id}, TenantId={customerOrdersDeletedEvent.TenantId}");
                throw;
            }
        }

        private static DeleteCustomerOrdersCommand CreateCommandFromEvent(CustomerOrdersDeletedEvent customerOrdersDeletedEvent)
        {
            return new DeleteCustomerOrdersCommand
            {
                Id = customerOrdersDeletedEvent.Id,
                TenantId = customerOrdersDeletedEvent.TenantId,
            };
        }
    }

}
