using MediatR;
using MultiTenantCQRS.Contract.Event.Events.CustomerOrders;
using MultiTenantCQRS.Write.Application.Commands.Order;
using MultiTenantCQRS.Write.Application.Transport.Response.Order;
using MultiTenantCQRS.Write.Domain.Interfaces.Bus;
using MultiTenantCQRS.Write.Domain.Interfaces.Repositories;
using MultiTenantCQRS.Write.Domain.Services;
using MultiTenantCQRS.Write.SqlServer.Services;

namespace MultiTenantCQRS.Write.Application.CommandHandlers.Order
{
    public sealed class CreateOrderCommandHandler(
        ServiceFactoryRepository serviceFactoryRepository,
        TenantProvider tenantProvider,
        IEventBus eventBus) : IRequestHandler<CreateOrderCommand, CreateOrderResponse>
    {
        private readonly ServiceFactoryRepository _serviceFactoryRepository = serviceFactoryRepository;
        private ICustomerRepository _customerRepository;
        private IOrderRepository _orderRepository;
        private readonly TenantProvider _tenantProvider = tenantProvider;
        private readonly IEventBus _eventBus = eventBus;

        public async Task<CreateOrderResponse> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            _customerRepository = _serviceFactoryRepository.GetInstanceRepository<ICustomerRepository>();
            _orderRepository = _serviceFactoryRepository.GetInstanceRepository<IOrderRepository>();

            var customerRegistered = await _customerRepository.GetByIdAsync(request.CustomerId) ?? throw new InvalidOperationException($"Customer with ID {request.CustomerId} not found. Operation cannot proceed without a valid customer.");

            var order = CreateOrder(request);

            var orderCreated = await _orderRepository.AddAsync(order);

            var ordersRegistered = await _orderRepository.GetByCustomerIdAsync(customerRegistered.Id);

            await PublishNewOrderCreatedEventAsync(customerRegistered, ordersRegistered);

            return MapToResponse(orderCreated);
        }

        private static CreateOrderResponse MapToResponse(Domain.Entities.Order orderCreated)
        {
            return new CreateOrderResponse(orderCreated.Id, orderCreated.Date, orderCreated.TotalAmount, orderCreated.CustomerId);
        }

        private static Domain.Entities.Order CreateOrder(CreateOrderCommand request)
        {
            return new Domain.Entities.Order
            {
                Date = request.OrderDate,
                TotalAmount = request.TotalAmount,
                CustomerId = request.CustomerId
            };
        }

        private async Task<CustomerOrdersCreatedOrUpdateEvent> PublishNewOrderCreatedEventAsync(Domain.Entities.Customer customerRegistered, IList<Domain.Entities.Order> ordersRegistered)
        {
            var @event = new CustomerOrdersCreatedOrUpdateEvent
            {
                TenantId = _tenantProvider.GetTenantId(),
                Id = customerRegistered.Id,
                Name = customerRegistered.Name,
                Email = customerRegistered.Email,
                Orders = ordersRegistered.Select(o => new CustomerOrdersCreatedOrUpdateEvent.Order
                {
                    Id = o.Id,
                    Date = o.Date,
                    TotalAmount = o.TotalAmount
                }).ToList()
            };

            await _eventBus.PublishAsync(@event);

            return @event;
        }
    }
}
