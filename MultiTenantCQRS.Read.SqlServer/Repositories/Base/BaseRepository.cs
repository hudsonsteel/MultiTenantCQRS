using Microsoft.Extensions.Options;
using MultiTenantCQRS.Read.Domain.Interfaces.Entities;
using MultiTenantCQRS.Read.SqlServer.Configurations;
using NHibernate;
using NHibernate.Linq;

namespace MultiTenantCQRS.Read.SqlServer.Repositories.Base
{
    public class BaseRepository<TEntity> where TEntity : class, IEntity
    {
        protected readonly SqlServerOption _sqlServerOption;
        private readonly ISessionFactory _sessionFactory;

        protected BaseRepository(IOptions<SqlServerOption> sqlServerOption, string schema)
        {
            _sqlServerOption = sqlServerOption.Value;

            var configuration = ModelMapperSetup.ConfigureMappings<TEntity>(_sqlServerOption.DefaultConnection, schema);

            _sessionFactory = configuration.BuildSessionFactory();
        }

        protected async Task<ITransaction> CreateTransactionAsync()
        {
            var session = await OpenSessionAsync();
            return session.BeginTransaction();
        }

        protected async Task<ISession> OpenSessionAsync()
        {
            return await Task.FromResult(_sessionFactory.OpenSession());
        }

        public virtual async Task<TEntity> GetByIdAsync(int id)
        {
            using var session = await OpenSessionAsync();
            return await session.GetAsync<TEntity>(id);
        }

        public async Task<TEntity> AddAsync(TEntity entity)
        {
            using var session = await OpenSessionAsync();
            using var transaction = session.BeginTransaction();

            try
            {
                await session.SaveAsync(entity);
                await transaction.CommitAsync();

                return entity;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<bool> RemoveAsync(TEntity entity)
        {
            using var session = await OpenSessionAsync();
            using var transaction = session.BeginTransaction();

            try
            {
                await session.DeleteAsync(entity);
                await transaction.CommitAsync();

                return true;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<TEntity> UpdateAsync(TEntity entity)
        {
            using var session = await OpenSessionAsync();
            using var transaction = session.BeginTransaction();

            try
            {
                await session.UpdateAsync(entity);
                await transaction.CommitAsync();

                return entity;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            using var session = await OpenSessionAsync();
            var query = session.Query<TEntity>();
            return await query.ToListAsync();
        }
    }
}
