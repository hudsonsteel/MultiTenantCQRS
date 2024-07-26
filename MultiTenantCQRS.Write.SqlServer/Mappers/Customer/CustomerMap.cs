using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace MultiTenantCQRS.Write.SqlServer.Mappers.Customer
{
    public sealed class CustomerMap : ClassMapping<Domain.Entities.Customer>
    {
        public CustomerMap(string schema)
        {
            Table($"{schema}.Customer");

            Id(x => x.Id, map =>
            {
                map.Generator(Generators.Identity);
            });

            Property(x => x.Name, map => { map.Length(100); map.NotNullable(true); });
            Property(x => x.Email, map => { map.Length(100); map.NotNullable(true); });
        }
    }
}
