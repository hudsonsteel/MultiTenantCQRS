namespace MultiTenantCQRS.Write.SqlServer.Configurations
{
    public sealed record class SqlServerOption
    {
        public string DefaultConnection { get; init; }
    }
}
