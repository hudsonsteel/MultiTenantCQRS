using MediatR;
using MultiTenantCQRS.Read.Domain.Dtos;

namespace MultiTenantCQRS.Read.Application.Commands.CustomerOrders
{
    public sealed record class CreateOrUpdateCustomerOrdersCommand : IRequest<Unit>
    {
        public int TenantId { get; init; }
        public int Id { get; init; }
        public string Name { get; init; }
        public string Email { get; init; }
        public IReadOnlyCollection<Order> Orders { get; init; }
    }
}
