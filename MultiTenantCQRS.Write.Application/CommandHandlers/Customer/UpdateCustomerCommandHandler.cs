using MediatR;
using MultiTenantCQRS.Contract.Event.Events.CustomerOrders;
using MultiTenantCQRS.Write.Application.Commands.Customer;
using MultiTenantCQRS.Write.Application.Transport.Response.Customer;
using MultiTenantCQRS.Write.Application.Transport.Response.Order;
using MultiTenantCQRS.Write.Domain.Interfaces.Bus;
using MultiTenantCQRS.Write.Domain.Interfaces.Repositories;
using MultiTenantCQRS.Write.Domain.Services;
using MultiTenantCQRS.Write.SqlServer.Services;


namespace MultiTenantCQRS.Write.Application.CommandHandlers.Customer
{
    public sealed class UpdateCustomerCommandHandler(
        ServiceFactoryRepository serviceFactoryRepository,
        TenantProvider tenantProvider,
        IEventBus eventBus) : IRequestHandler<UpdateCustomerCommand, UpdateCustomerResponse>
    {
        private readonly ServiceFactoryRepository _serviceFactoryRepository = serviceFactoryRepository;
        private ICustomerRepository _customerRepository;
        private IOrderRepository _orderRepository;
        private readonly TenantProvider _tenantProvider = tenantProvider;
        private readonly IEventBus _eventBus = eventBus;

        public async Task<UpdateCustomerResponse> Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
        {
            _customerRepository = _serviceFactoryRepository.GetInstanceRepository<ICustomerRepository>();
            _orderRepository = _serviceFactoryRepository.GetInstanceRepository<IOrderRepository>();

            var customer = await GetCustomerByIdAsync(request.Id);

            UpdateCustomerDetails(customer, request);

            var ordersRegistered = await UpdateOrAddOrdersAsync(request.Orders, request.Id);

            var customerUpdated = await _customerRepository.UpdateAsync(customer);

            await PublishCustomerUpdateEventAsync(request);

            return MapToResponse(customerUpdated, ordersRegistered);
        }

        private static UpdateCustomerResponse MapToResponse(Domain.Entities.Customer customer, IList<Domain.Entities.Order> orders)
        {
            ArgumentNullException.ThrowIfNull(customer, nameof(customer));
            ArgumentNullException.ThrowIfNull(orders, nameof(orders));

            return new UpdateCustomerResponse
            (
                customer.Id,
                customer.Name,
                customer.Email,
                orders.Select(o => new UpdateOrderResponse
                (
                    o.Id,
                    o.Date,
                    o.TotalAmount,
                    customer.Id
                )).ToList()
            );
        }

        private async Task<Domain.Entities.Customer> GetCustomerByIdAsync(int customerId)
        {
            var customer = await _customerRepository.GetByIdAsync(customerId);

            return customer ?? throw new Exception($"Customer with ID {customerId} not found.");
        }

        private static void UpdateCustomerDetails(Domain.Entities.Customer customer, UpdateCustomerCommand request)
        {
            customer.Name = request.Name;
            customer.Email = request.Email;
        }

        private async Task<IList<Domain.Entities.Order>> UpdateOrAddOrdersAsync(IEnumerable<UpdateCustomerCommand.Order> orders, int customerId)
        {
            var ordersRegistered = new List<Domain.Entities.Order>();

            foreach (var order in orders)
            {
                var existingOrder = await _orderRepository.GetByIdAsync(order.Id);

                if (existingOrder != null)
                {
                    UpdateExistingOrder(existingOrder, order);
                    var orderRegistered = await _orderRepository.UpdateAsync(existingOrder);
                    ordersRegistered.Add(orderRegistered);
                }
                else
                {
                    throw new InvalidOperationException($"Order with ID {order.Id} not found for update.");
                }
            }
            return ordersRegistered;
        }

        private static void UpdateExistingOrder(Domain.Entities.Order existingOrder, UpdateCustomerCommand.Order order)
        {
            existingOrder.Date = order.Date;
            existingOrder.TotalAmount = order.TotalAmount;
        }

        private async Task PublishCustomerUpdateEventAsync(UpdateCustomerCommand request)
        {
            var @event = new CustomerOrdersCreatedOrUpdateEvent
            {
                TenantId = _tenantProvider.GetTenantId(),
                Id = request.Id,
                Name = request.Name,
                Email = request.Email,
                Orders = request.Orders.Select(o => new CustomerOrdersCreatedOrUpdateEvent.Order
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
