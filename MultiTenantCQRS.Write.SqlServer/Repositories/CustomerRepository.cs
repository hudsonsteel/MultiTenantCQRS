using Microsoft.Extensions.Options;
using MultiTenantCQRS.Write.Domain.Entities;
using MultiTenantCQRS.Write.Domain.Interfaces.Repositories;
using MultiTenantCQRS.Write.SqlServer.Configurations;
using MultiTenantCQRS.Write.SqlServer.Repositories.Base;

namespace MultiTenantCQRS.Write.SqlServer.Repositories
{
    public sealed class CustomerRepository(IOptions<SqlServerOption> sqlServerOption, string schema) : BaseRepository<Customer>(sqlServerOption, schema), ICustomerRepository
    {
    }
}
