using MultiTenantCQRS.Read.Domain.Interfaces.Entities;
using MultiTenantCQRS.Read.SqlServer.Mappers.Customer;
using NHibernate.Cfg;
using NHibernate.Mapping.ByCode;

namespace MultiTenantCQRS.Read.SqlServer.Configurations
{
    public class ModelMapperSetup
    {
        public static Configuration ConfigureMappings<TEntity>(string connectionString, string schema) where TEntity : class, IEntity
        {
            var configuration = new Configuration();
            configuration.SetProperty(NHibernate.Cfg.Environment.Dialect, "NHibernate.Dialect.MsSql2012Dialect");
            configuration.SetProperty(NHibernate.Cfg.Environment.ConnectionString, connectionString);
            configuration.SetProperty(NHibernate.Cfg.Environment.ShowSql, "true");
            configuration.SetProperty(NHibernate.Cfg.Environment.FormatSql, "true");

            var mapper = new ModelMapper();
            mapper.AddMapping(new CustomerMap(schema));

            var mappings = mapper.CompileMappingForAllExplicitlyAddedEntities();
            configuration.AddDeserializedMapping(mappings, typeof(TEntity).Assembly.FullName);

            return configuration;
        }
    }
}
