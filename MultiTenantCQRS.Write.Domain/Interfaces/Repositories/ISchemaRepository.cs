namespace MultiTenantCQRS.Write.Domain.Interfaces.Repositories
{
    public interface ISchemaRepository
    {
        Task InitializeAsync(CancellationToken cancellationToken);
    }
}
