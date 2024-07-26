namespace MultiTenantCQRS.Read.Domain.Interfaces.Repositories
{
    public interface ISchemaRepository
    {
        Task InitializeAsync(CancellationToken cancellationToken);
    }
}
