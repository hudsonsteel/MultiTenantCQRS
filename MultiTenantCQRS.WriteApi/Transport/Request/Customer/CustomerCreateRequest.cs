using MultiTenantCQRS.WriteApi.Transport.Request.Order;
using System.Collections.Immutable;

namespace MultiTenantCQRS.WriteApi.Transport.Request.Customer
{
    public sealed record class CustomerCreateRequest
    {
        public string Name { get; init; }
        public string Email { get; init; }
        public ImmutableList<CreateOrderRequest> Orders { get; init; }
    }
}
