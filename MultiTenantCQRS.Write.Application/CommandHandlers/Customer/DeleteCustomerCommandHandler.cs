using MediatR;
using MultiTenantCQRS.Contract.Event.Events.CustomerOrders;
using MultiTenantCQRS.Write.Application.Commands.Customer;
using MultiTenantCQRS.Write.Domain.Interfaces.Bus;
using MultiTenantCQRS.Write.Domain.Interfaces.Repositories;
using MultiTenantCQRS.Write.Domain.Services;
using MultiTenantCQRS.Write.SqlServer.Services;

namespace MultiTenantCQRS.Write.Application.CommandHandlers.Customer
{
    public sealed class DeleteCustomerCommandHandler(
        ServiceFactoryRepository serviceFactoryRepository,
        TenantProvider tenantProvider,
        IEventBus eventBus) : IRequestHandler<DeleteCustomerCommand, Unit>
    {
        private readonly ServiceFactoryRepository _serviceFactoryRepository = serviceFactoryRepository;
        private ICustomerRepository _customerRepository;
        private IOrderRepository _orderRepository;
        private readonly TenantProvider _tenantProvider = tenantProvider;
        private readonly IEventBus _eventBus = eventBus;

        public async Task<Unit> Handle(DeleteCustomerCommand request, CancellationToken cancellationToken)
        {
            _customerRepository = _serviceFactoryRepository.GetInstanceRepository<ICustomerRepository>();
            _orderRepository = _serviceFactoryRepository.GetInstanceRepository<IOrderRepository>();

            var customer = await _customerRepository.GetByIdAsync(request.Id) ?? throw new Exception($"Customer with ID {request.Id} not found.");

            var orders = await _orderRepository.GetByCustomerIdAsync(request.Id) ?? throw new Exception($"Customer Orders with ID {request.Id} not found.");

            orders.Select(async order =>
            {
                await _orderRepository.RemoveAsync(order);
            });

            await _customerRepository.RemoveAsync(customer);

            var @event = new CustomerOrdersDeletedEvent
            {
                Id = request.Id,
                TenantId = _tenantProvider.GetTenantId()
            };

            await _eventBus.PublishAsync(@event);

            return Unit.Value;
        }
    }
}
