using Microsoft.Extensions.Options;
using MultiTenantCQRS.Read.Domain.Entities;
using MultiTenantCQRS.Read.Domain.Interfaces.Repositories;
using MultiTenantCQRS.Read.SqlServer.Configurations;
using MultiTenantCQRS.Read.SqlServer.Repositories.Base;

namespace MultiTenantCQRS.Read.SqlServer.Repositories
{
    public sealed class CustomerOrdersRepository(IOptions<SqlServerOption> sqlServerOption, string schema) : BaseRepository<CustomerOrders>(sqlServerOption, schema), ICustomerOrdersRepository
    {
    }
}
