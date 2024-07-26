namespace MultiTenantCQRS.WriteApi.Transport.Request.Order
{
    public sealed record class UpdateOrderRequest
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public double TotalAmount { get; set; }
        public DateTime Date { get; set; }
    }
}
