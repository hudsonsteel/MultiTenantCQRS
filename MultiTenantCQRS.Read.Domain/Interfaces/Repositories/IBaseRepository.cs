using MultiTenantCQRS.Read.Domain.Interfaces.Entities;

namespace MultiTenantCQRS.Read.Domain.Interfaces.Repositories
{
    public interface IBaseRepository<TEntity> where TEntity : class, IEntity
    {
        public Task<TEntity> GetByIdAsync(int id);
        public Task<TEntity> AddAsync(TEntity entity);
        public Task<bool> RemoveAsync(TEntity entity);
        public Task<TEntity> UpdateAsync(TEntity entity);
        public Task<IEnumerable<TEntity>> GetAllAsync();
    }
}
