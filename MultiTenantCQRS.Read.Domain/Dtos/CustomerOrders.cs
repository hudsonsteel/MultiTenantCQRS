namespace MultiTenantCQRS.Read.Domain.Dtos
{
    public sealed record class CustomerOrders
    {
        public CustomerOrders(int tenantId, int id, string name, string email, IReadOnlyCollection<Order> orders)
        {
            TenantId = tenantId;
            Id = id;
            Name = name;
            Email = email;
            Orders = orders;
        }

        public int TenantId { get; init; }
        public int Id { get; init; }
        public string Name { get; init; }
        public string Email { get; init; }
        public IReadOnlyCollection<Order> Orders { get; init; }
    }
}
