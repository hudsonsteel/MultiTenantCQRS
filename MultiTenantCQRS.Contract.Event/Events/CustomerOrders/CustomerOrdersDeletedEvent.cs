namespace MultiTenantCQRS.Contract.Event.Events.CustomerOrders
{
    public sealed record class CustomerOrdersDeletedEvent
    {
        public int Id { get; init; }
        public int TenantId { get; init; }
    }
}
