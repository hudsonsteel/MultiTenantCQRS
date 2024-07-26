using MediatR;
using MultiTenantCQRS.Write.Application.Transport.Response.Order;

namespace MultiTenantCQRS.Write.Application.Commands.Order
{
    public sealed record class UpdateOrderCommand : IRequest<UpdateOrderResponse>
    {
        public int Id { get; init; }
        public DateTime Date { get; init; }
        public double TotalAmount { get; init; }
        public int CustomerId { get; init; }
    }
}
