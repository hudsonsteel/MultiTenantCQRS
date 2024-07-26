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
    public sealed class UpdateOrderCommandHandler(
        ServiceFactoryRepository serviceFactoryRepository,
        TenantProvider tenantProvider,
        IEventBus eventBus) : IRequestHandler<UpdateOrderCommand, UpdateOrderResponse>
    {
        private readonly ServiceFactoryRepository _serviceFactoryRepository = serviceFactoryRepository;
        private ICustomerRepository _customerRepository;
        private IOrderRepository _orderRepository;
        private readonly TenantProvider _tenantProvider = tenantProvider;
        private readonly IEventBus _eventBus = eventBus;

        public async Task<UpdateOrderResponse> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
        {
            _customerRepository = _serviceFactoryRepository.GetInstanceRepository<ICustomerRepository>();
            _orderRepository = _serviceFactoryRepository.GetInstanceRepository<IOrderRepository>();

            var order = await GetOrderByIdAndByCustomerIdAsync(request.Id, request.CustomerId);

            UpdateOrder(order, request);

            var orderUpdated = await SaveOrderAsync(order);

            var customerRegistered = await _customerRepository.GetByIdAsync(request.CustomerId) ?? throw new InvalidOperationException($"Customer with ID {request.CustomerId} not found. Operation cannot proceed without a valid customer.");

            var ordersRegistered = await _orderRepository.GetByCustomerIdAsync(customerRegistered.Id);

            await PublishOrderUpdatedEventAsync(customerRegistered, ordersRegistered);

            return MapToResponse(orderUpdated);
        }

        private async Task<Domain.Entities.Order> GetOrderByIdAndByCustomerIdAsync(int orderId, int customerId)
        {
            var order = await _orderRepository.GetByIdAsync(orderId);

            if (order == null)
            {
                throw new KeyNotFoundException($"Order with ID {orderId} not found.");
            }

            if (order.CustomerId != customerId)
            {
                throw new InvalidOperationException($"Order with ID {orderId} belongs to a different customer. Expected Customer ID: {customerId}, but found Customer ID: {order.CustomerId}.");
            }

            return order;
        }

        private static UpdateOrderResponse MapToResponse(Domain.Entities.Order order)
        {
            return new UpdateOrderResponse
            (
                order.Id,
                order.Date,
                order.TotalAmount,
                order.CustomerId
            );
        }

        private static void UpdateOrder(Domain.Entities.Order order, UpdateOrderCommand request)
        {
            order.Date = request.Date;
            order.TotalAmount = request.TotalAmount;
            order.CustomerId = request.CustomerId;
        }

        private async Task<Domain.Entities.Order> SaveOrderAsync(Domain.Entities.Order order)
        {
            return await _orderRepository.UpdateAsync(order);
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
