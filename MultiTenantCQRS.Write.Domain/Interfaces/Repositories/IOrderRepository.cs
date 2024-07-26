using MultiTenantCQRS.Write.Domain.Entities;

namespace MultiTenantCQRS.Write.Domain.Interfaces.Repositories
{
    public interface IOrderRepository : IBaseRepository<Order>
    {
        Task<IList<Order>> GetByCustomerIdAsync(int customerId);
    }
}
