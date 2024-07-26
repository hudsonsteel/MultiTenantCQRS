namespace MultiTenantCQRS.Read.Domain.Dtos
{
    public sealed record class Order
    {
        public Order(int id, DateTime date, double totalAmount)
        {
            Id = id;
            Date = date;
            TotalAmount = totalAmount;
        }

        public int Id { get; init; }
        public DateTime Date { get; init; }
        public double TotalAmount { get; init; }
    }
}
