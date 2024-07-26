namespace MultiTenantCQRS.WriteApi.Transport.Request.Order
{
    public sealed record class CreateOrderRequest
    {
        public int CustomerId { get; set; }
        public double TotalAmount { get; set; }
        public DateTime Date { get; set; }
    }
}
