﻿namespace MultiTenantCQRS.Write.Application.Transport.Response.Order
{
    public sealed record class UpdateOrderResponse
    {
        public int Id { get; init; }
        public DateTime Date { get; init; }
        public double TotalAmount { get; init; }
        public int CustomerId { get; init; }

        public UpdateOrderResponse(int id, DateTime date, double totalAmount, int customerId) =>
            (Id, Date, TotalAmount, CustomerId) = (id, date, totalAmount, customerId);
    }
}
