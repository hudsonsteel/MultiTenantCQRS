using MultiTenantCQRS.Write.Domain.Interfaces.Entities;

namespace MultiTenantCQRS.Write.Domain.Entities
{
    public class Order : IEntity
    {
        public virtual int Id { get; set; }
        public virtual DateTime Date { get; set; }
        public virtual double TotalAmount { get; set; }
        public virtual int CustomerId { get; set; }
    }
}
