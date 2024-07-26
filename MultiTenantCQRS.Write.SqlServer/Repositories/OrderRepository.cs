using Microsoft.Extensions.Options;
using MultiTenantCQRS.Write.Domain.Entities;
using MultiTenantCQRS.Write.Domain.Interfaces.Repositories;
using MultiTenantCQRS.Write.SqlServer.Configurations;
using MultiTenantCQRS.Write.SqlServer.Repositories.Base;
using NHibernate.Linq;

namespace MultiTenantCQRS.Write.SqlServer.Repositories
{
    public sealed class OrderRepository(IOptions<SqlServerOption> sqlServerOption, string schema) : BaseRepository<Order>(sqlServerOption, schema), IOrderRepository
    {
        public async Task<IList<Order>> GetByCustomerIdAsync(int customerId)
        {
            using var session = await OpenSessionAsync();
            return await session.Query<Order>()
                                .Where(o => o.CustomerId == customerId)
                                .ToListAsync();
        }
    }
}
