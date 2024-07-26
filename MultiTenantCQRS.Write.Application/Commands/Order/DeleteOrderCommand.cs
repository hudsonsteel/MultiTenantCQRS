using MediatR;

namespace MultiTenantCQRS.Write.Application.Commands.Order
{
    public sealed record class DeleteOrderCommand(int CustomerId, int OrderId) : IRequest<Unit>
    {
        public int CustomerId { get; init; } = CustomerId;
        public int OrderId { get; init; } = OrderId;
    }
}
