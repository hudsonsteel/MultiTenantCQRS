using MultiTenantCQRS.Write.Domain.Interfaces.Entities;
using MultiTenantCQRS.Write.SqlServer.Mappers.Customer;
using MultiTenantCQRS.Write.SqlServer.Mappers.Order;
using NHibernate.Cfg;
using NHibernate.Mapping.ByCode;

namespace MultiTenantCQRS.Write.SqlServer.Configurations
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
            mapper.AddMapping(new OrderMap(schema));

            var mappings = mapper.CompileMappingForAllExplicitlyAddedEntities();
            configuration.AddDeserializedMapping(mappings, typeof(TEntity).Assembly.FullName);

            return configuration;
        }
    }

}
