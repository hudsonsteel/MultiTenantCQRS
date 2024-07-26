using MediatR;
using Microsoft.Extensions.Logging;
using MultiTenantCQRS.Read.Application.Commands.CustomerOrders;
using MultiTenantCQRS.Read.Domain.Interfaces.Repositories;
using MultiTenantCQRS.Read.SqlServer.Services;

namespace MultiTenantCQRS.Read.Application.CommandHandlers.CustomerOrders
{
    public class DeleteCustomerOrdersCommandHandler(
        ServiceFactoryRepository serviceFactoryRepository,
        ILogger<DeleteCustomerOrdersCommandHandler> logger) : IRequestHandler<DeleteCustomerOrdersCommand, Unit>
    {
        private readonly ILogger<DeleteCustomerOrdersCommandHandler> _logger = logger;
        private readonly ServiceFactoryRepository _serviceFactoryRepository = serviceFactoryRepository;

        public async Task<Unit> Handle(DeleteCustomerOrdersCommand request, CancellationToken cancellationToken)
        {
            var repository = _serviceFactoryRepository.GetInstanceRepository<ICustomerOrdersRepository>(request.TenantId);

            var existingOrder = await repository.GetByIdAsync(request.Id);

            if (existingOrder != null)
            {
                await repository.RemoveAsync(existingOrder);
                _logger.LogInformation($"Removed CustomerOrders: {existingOrder.CorrelationId}, TenantId: {request.TenantId}");
            }
            else
            {
                _logger.LogWarning($"CustomerOrders with Id {request.Id} not found for TenantId: {request.TenantId}");
            }

            return Unit.Value;
        }
    }
}
