namespace MultiTenantCQRS.Read.Domain.Interfaces.Entities
{
    public interface IEntity
    {
        public int CorrelationId { get; set; }
    }
}
