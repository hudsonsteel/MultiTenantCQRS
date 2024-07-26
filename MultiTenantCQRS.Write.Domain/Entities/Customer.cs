using MultiTenantCQRS.Write.Domain.Interfaces.Entities;

namespace MultiTenantCQRS.Write.Domain.Entities
{
    public class Customer : IEntity
    {
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
        public virtual string Email { get; set; }
    }
}
