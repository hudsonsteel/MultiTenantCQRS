using MultiTenantCQRS.Read.Domain.Interfaces.Entities;

namespace MultiTenantCQRS.Read.Domain.Entities
{
    public class CustomerOrders : IEntity
    {
        public virtual int CorrelationId { get; set; }
        public virtual string Json { get; set; }
    }
}
