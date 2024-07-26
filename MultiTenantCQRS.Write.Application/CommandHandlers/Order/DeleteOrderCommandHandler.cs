using MediatR;
using MultiTenantCQRS.Contract.Event.Events.CustomerOrders;
using MultiTenantCQRS.Write.Application.Commands.Order;
using MultiTenantCQRS.Write.Domain.Interfaces.Bus;
using MultiTenantCQRS.Write.Domain.Interfaces.Repositories;
using MultiTenantCQRS.Write.Domain.Services;
using MultiTenantCQRS.Write.SqlServer.Services;

namespace MultiTenantCQRS.Write.Application.CommandHandlers.Order
{
    public sealed class DeleteOrderCommandHandler(
        ServiceFactoryRepository serviceFactoryRepository,
        TenantProvider tenantProvider,
        IEventBus eventBus) : IRequestHandler<DeleteOrderCommand, Unit>
    {
        private readonly ServiceFactoryRepository _serviceFactoryRepository = serviceFactoryRepository;
        private ICustomerRepository _customerRepository;
        private IOrderRepository _orderRepository;
        private readonly TenantProvider _tenantProvider = tenantProvider;
        private readonly IEventBus _eventBus = eventBus;

        public async Task<Unit> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
        {
            _customerRepository = _serviceFactoryRepository.GetInstanceRepository<ICustomerRepository>();
            _orderRepository = _serviceFactoryRepository.GetInstanceRepository<IOrderRepository>();

            var order = await _orderRepository.GetByIdAsync(request.OrderId) ?? throw new Exception($"Customer Orders with ID {request.OrderId} not found.");

            await _orderRepository.RemoveAsync(order);

            var customerRegistered = await _customerRepository.GetByIdAsync(request.CustomerId) ?? throw new InvalidOperationException($"Customer with ID {request.CustomerId} not found. Operation cannot proceed without a valid customer.");

            var ordersRegistered = await _orderRepository.GetByCustomerIdAsync(customerRegistered.Id);

            await PublishOrderUpdatedEventAsync(customerRegistered, ordersRegistered);

            return Unit.Value;
        }
        private async Task PublishOrderUpdatedEventAsync(Domain.Entities.Customer customer, IList<Domain.Entities.Order> orders)
        {
            var @event = new CustomerOrdersCreatedOrUpdateEvent
            {
                TenantId = _tenantProvider.GetTenantId(),
                Id = customer.Id,
                Name = customer.Name,
                Email = customer.Email,
                Orders = orders.Select(o => new CustomerOrdersCreatedOrUpdateEvent.Order
                {
                    Id = o.Id,
                    Date = o.Date,
                    TotalAmount = o.TotalAmount
                }).ToList()
            };

            await _eventBus.PublishAsync(@event);
        }
    }
}
