using MultiTenantCQRS.Write.Application.Transport.Response.Order;

namespace MultiTenantCQRS.WriteApi.Transport.Response.Customer
{
    public sealed record class CustomerCreateResponse
    {
        public int Id { get; init; }
        public string Name { get; init; }
        public string Email { get; init; }
        public IReadOnlyList<CreateOrderResponse> Orders { get; init; }

        public CustomerCreateResponse(int id, string name, string email, IReadOnlyList<CreateOrderResponse> orders)
        {
            Id = id;
            Name = name;
            Email = email;
            Orders = orders;
        }
    }
}
