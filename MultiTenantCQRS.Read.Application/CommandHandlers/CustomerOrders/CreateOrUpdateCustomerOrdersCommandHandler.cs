using MediatR;
using Microsoft.Extensions.Logging;
using MultiTenantCQRS.Read.Application.Commands.CustomerOrders;
using MultiTenantCQRS.Read.Domain.Interfaces.Repositories;
using MultiTenantCQRS.Read.SqlServer.Services;
using Newtonsoft.Json;

namespace MultiTenantCQRS.Read.Application.CommandHandlers.CustomerOrders
{
    public class CreateOrUpdateCustomerOrdersCommandHandler(
        ILogger<CreateOrUpdateCustomerOrdersCommandHandler> logger,
        ServiceFactoryRepository serviceFactoryRepository) : IRequestHandler<CreateOrUpdateCustomerOrdersCommand, Unit>
    {
        private readonly ILogger<CreateOrUpdateCustomerOrdersCommandHandler> _logger = logger;
        private readonly ServiceFactoryRepository _serviceFactoryRepository = serviceFactoryRepository;

        public async Task<Unit> Handle(CreateOrUpdateCustomerOrdersCommand request, CancellationToken cancellationToken)
        {
            var repository = _serviceFactoryRepository.GetInstanceRepository<ICustomerOrdersRepository>(request.TenantId);

            var existingOrder = await repository.GetByIdAsync(request.Id);

            var entity = CreateEntityFromRequest(request, existingOrder);

            if (existingOrder != null)
            {
                await repository.UpdateAsync(entity);
                _logger.LogInformation($"Updated CustomerOrders: Id={request.Id}, TenantId={request.TenantId}");
            }
            else
            {
                await repository.AddAsync(entity);
                _logger.LogInformation($"Added CustomerOrders: Id={request.Id}, TenantId={request.TenantId}");
            }

            return Unit.Value;
        }

        private static Domain.Entities.CustomerOrders CreateEntityFromRequest(
            CreateOrUpdateCustomerOrdersCommand request,
            Domain.Entities.CustomerOrders existingOrder)
        {
            var json = new Domain.Dtos.CustomerOrders
            (
                request.TenantId,
                request.Id,
                request.Name,
                request.Email,
                request.Orders
            );

            return new Domain.Entities.CustomerOrders
            {
                CorrelationId = request.Id,
                Json = JsonConvert.SerializeObject(json)
            };
        }
    }
}
