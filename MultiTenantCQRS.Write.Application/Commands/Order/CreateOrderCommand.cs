using MediatR;
using MultiTenantCQRS.Write.Application.Transport.Response.Order;

namespace MultiTenantCQRS.Write.Application.Commands.Order
{
    public sealed record class CreateOrderCommand : IRequest<CreateOrderResponse>
    {
        public DateTime OrderDate { get; set; }
        public double TotalAmount { get; set; }
        public int CustomerId { get; set; }
    }
}
