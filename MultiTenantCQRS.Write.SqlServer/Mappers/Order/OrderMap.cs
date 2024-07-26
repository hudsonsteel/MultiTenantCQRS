using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using NHibernate.Type;

namespace MultiTenantCQRS.Write.SqlServer.Mappers.Order
{
    public sealed class OrderMap : ClassMapping<Domain.Entities.Order>
    {
        public OrderMap(string schema)
        {
            Table($"{schema}.[Order]");

            Id(x => x.Id, map =>
            {
                map.Generator(Generators.Identity);
            });

            Property(x => x.CustomerId, map => { map.NotNullable(true); });
            Property(x => x.TotalAmount, map =>
            {
                map.NotNullable(true);
                map.Precision(18);
                map.Scale(2);
            });

            Property(x => x.Date, map =>
            {
                map.NotNullable(true);
                map.Type<DateTimeType>();
            });
        }
    }
}
