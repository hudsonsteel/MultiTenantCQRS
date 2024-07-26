using MediatR;
using MultiTenantCQRS.Contract.Event.Events.CustomerOrders;
using MultiTenantCQRS.Write.Application.Commands.Customer;
using MultiTenantCQRS.Write.Application.Transport.Response.Order;
using MultiTenantCQRS.Write.Domain.Interfaces.Bus;
using MultiTenantCQRS.Write.Domain.Interfaces.Repositories;
using MultiTenantCQRS.Write.Domain.Services;
using MultiTenantCQRS.Write.SqlServer.Services;
using MultiTenantCQRS.WriteApi.Transport.Response.Customer;

namespace MultiTenantCQRS.Write.Application.CommandHandlers.Customer
{
    public sealed class CreateCustomerCommandHandler(
        ServiceFactoryRepository serviceFactoryRepository,
        TenantProvider tenantProvider,
        IEventBus eventBus) : IRequestHandler<CreateCustomerCommand, CustomerCreateResponse>
    {
        private readonly ServiceFactoryRepository _serviceFactoryRepository = serviceFactoryRepository;
        private readonly TenantProvider _tenantProvider = tenantProvider;
        private readonly IEventBus _eventBus = eventBus;
        private ICustomerRepository _customerRepository;
        private IOrderRepository _orderRepository;

        public async Task<CustomerCreateResponse> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
        {
            _customerRepository = _serviceFactoryRepository.GetInstanceRepository<ICustomerRepository>();
            _orderRepository = _serviceFactoryRepository.GetInstanceRepository<IOrderRepository>();

            var customer = new Domain.Entities.Customer { Name = request.Name, Email = request.Email };

            var customerRegistered = await _customerRepository.AddAsync(customer);

            var ordersRegistered = await AddOrdersAsync(request.Orders, customerRegistered.Id);

            var @event = await CreateAndPublishEventAsync(customerRegistered, ordersRegistered);

            return MapToResponse(@event);
        }

        private async Task<IList<Domain.Entities.Order>> AddOrdersAsync(IReadOnlyList<CreateCustomerCommand.Order> orders, int customerId)
        {
            var ordersRegistered = new List<Domain.Entities.Order>();

            foreach (var orderRequest in orders)
            {
                var order = new Domain.Entities.Order
                {
                    Date = orderRequest.Date,
                    TotalAmount = orderRequest.TotalAmount,
                    CustomerId = customerId
                };

                ordersRegistered.Add(await _orderRepository.AddAsync(order));
            }

            return ordersRegistered;

        }

        private async Task<CustomerOrdersCreatedOrUpdateEvent> CreateAndPublishEventAsync(Domain.Entities.Customer customerRegistered, IList<Domain.Entities.Order> ordersRegistered)
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

        private static CustomerCreateResponse MapToResponse(CustomerOrdersCreatedOrUpdateEvent createEvent)
        {
            var response = new CustomerCreateResponse
            (
                 createEvent.Id,
                 createEvent.Name,
                 createEvent.Email,
                 createEvent.Orders.Select(o => new CreateOrderResponse
                 (
                     o.Id,
                     o.Date,
                     o.TotalAmount,
                     createEvent.Id
                 )).ToList()
            );

            return response;
        }
    }
}
