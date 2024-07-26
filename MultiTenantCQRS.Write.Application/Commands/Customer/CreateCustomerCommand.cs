using MediatR;
using MultiTenantCQRS.WriteApi.Transport.Response.Customer;

namespace MultiTenantCQRS.Write.Application.Commands.Customer
{
    public class CreateCustomerCommand : IRequest<CustomerCreateResponse>
    {
        public int Id { get; init; }
        public string Name { get; init; }
        public string Email { get; init; }
        public IReadOnlyList<Order> Orders { get; init; }

        public sealed record class Order
        {
            public int Id { get; init; }
            public DateTime Date { get; init; }
            public double TotalAmount { get; init; }
            public int CustomerId { get; init; }
        }
    }
}
