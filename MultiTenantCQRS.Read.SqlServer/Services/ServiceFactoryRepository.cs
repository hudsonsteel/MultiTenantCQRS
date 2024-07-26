using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MultiTenantCQRS.Read.Domain.Interfaces.Repositories;
using MultiTenantCQRS.Read.Domain.Services;
using MultiTenantCQRS.Read.SqlServer.Configurations;
using MultiTenantCQRS.Read.SqlServer.Repositories;

namespace MultiTenantCQRS.Read.SqlServer.Services
{
    public class ServiceFactoryRepository
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IOptions<SqlServerOption> _sqlServerOption;

        private readonly IDictionary<Type, Func<string, object>> _repositoryFactories;

        public ServiceFactoryRepository(IServiceScopeFactory scopeFactory, IOptions<SqlServerOption> sqlServerOption)
        {
            _scopeFactory = scopeFactory;
            _sqlServerOption = sqlServerOption;

            _repositoryFactories = new Dictionary<Type, Func<string, object>>
        {
            { typeof(ICustomerOrdersRepository), schema => new CustomerOrdersRepository(_sqlServerOption, schema) },
        };
        }

        public T GetInstanceRepository<T>(int tenantId = 0) where T : class
        {
            var scope = _scopeFactory.CreateScope();
            var tenantProvider = scope.ServiceProvider.GetRequiredService<TenantProvider>();
            tenantProvider.SetTenantId(tenantId);
            var schema = $"{tenantProvider.GetSchemaName()}_read";

            if (_repositoryFactories.TryGetValue(typeof(T), out var factory))
            {
                return factory(schema) as T;
            }

            throw new InvalidOperationException($"Repository type {typeof(T).Name} not recognized.");
        }
    }
}
