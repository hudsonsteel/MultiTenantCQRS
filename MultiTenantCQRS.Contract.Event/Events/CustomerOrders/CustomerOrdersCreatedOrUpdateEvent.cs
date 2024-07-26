namespace MultiTenantCQRS.Contract.Event.Events.CustomerOrders
{
    public record class CustomerOrdersCreatedOrUpdateEvent
    {
        public int TenantId { get; init; }
        public int Id { get; init; }
        public string Name { get; init; }
        public string Email { get; init; }

        public IReadOnlyCollection<Order> Orders { get; init; }

        public record class Order
        {
            public int Id { get; init; }
            public DateTime Date { get; init; }
            public double TotalAmount { get; init; }
        }
    }
}
