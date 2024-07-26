using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace MultiTenantCQRS.Read.SqlServer.Mappers.Customer
{
    public sealed class CustomerMap : ClassMapping<Domain.Entities.CustomerOrders>
    {
        public CustomerMap(string schema)
        {
            Table($"{schema}.CustomerOrders");

            Id(x => x.CorrelationId, map =>
            {
                map.Column("CorrelationId");
                map.Generator(Generators.Assigned);
            });

            Property(x => x.Json, map =>
            {
                map.Column("Json");
                map.Length(int.MaxValue);
                map.NotNullable(true);
            });
        }
    }
}
