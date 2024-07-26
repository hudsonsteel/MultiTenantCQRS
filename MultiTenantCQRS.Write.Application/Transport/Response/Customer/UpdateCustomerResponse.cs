using MultiTenantCQRS.Write.Application.Transport.Response.Order;

namespace MultiTenantCQRS.Write.Application.Transport.Response.Customer
{
    public sealed record class UpdateCustomerResponse
    {
        public int Id { get; init; }
        public string Name { get; init; }
        public string Email { get; init; }
        public IReadOnlyList<UpdateOrderResponse> Orders { get; init; }

        public UpdateCustomerResponse(int id, string name, string email, IReadOnlyList<UpdateOrderResponse> orders)
        {
            Id = id;
            Name = name;
            Email = email;
            Orders = orders;
        }
    }
}
