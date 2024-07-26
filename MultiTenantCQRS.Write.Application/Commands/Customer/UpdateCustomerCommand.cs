using MediatR;
using MultiTenantCQRS.Write.Application.Transport.Response.Customer;

namespace MultiTenantCQRS.Write.Application.Commands.Customer
{
    public sealed record class UpdateCustomerCommand : IRequest<UpdateCustomerResponse>
    {
        public int Id { get; init; }
        public string Name { get; init; }
        public string Email { get; init; }
        public IList<Order> Orders { get; init; }

        public class Order
        {
            public int Id { get; init; }
            public DateTime Date { get; init; }
            public double TotalAmount { get; init; }
            public int CustomerId { get; init; }
        }
    }
}
