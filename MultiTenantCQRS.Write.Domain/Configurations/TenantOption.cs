namespace MultiTenantCQRS.Write.Domain.Configurations
{
    public sealed record class TenantOption
    {
        public IReadOnlyList<Tenant> Tenants { get; init; }

        public sealed record class Tenant
        {
            public int TenantId { get; init; }
            public string SchemaName { get; init; }

            public Tenant(int tenantId, string schemaName)
            {
                TenantId = tenantId;
                SchemaName = schemaName;
            }
        }
    }
}
