using MediatR;

namespace MultiTenantCQRS.Read.Application.Commands.CustomerOrders
{
    public sealed record class DeleteCustomerOrdersCommand : IRequest<Unit>
    {
        public int Id { get; init; }
        public int TenantId { get; init; }
    }
}
