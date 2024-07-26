namespace MultiTenantCQRS.Read.SqlServer.Configurations
{
    public sealed record class SqlServerOption
    {
        public string DefaultConnection { get; init; }
    }
}
