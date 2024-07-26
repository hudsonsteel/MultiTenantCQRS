using MultiTenantCQRS.WriteApi.Transport.Request.Order;
using System.Collections.Immutable;

namespace MultiTenantCQRS.WriteApi.Transport.Request.Customer
{
    public sealed record class CustomerUpdateRequest
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public ImmutableList<UpdateOrderRequest> Orders { get; set; }
    }
}
