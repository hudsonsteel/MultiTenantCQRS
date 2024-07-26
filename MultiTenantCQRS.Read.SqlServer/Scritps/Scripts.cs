namespace MultiTenantCQRS.Read.SqlServer.Scritps
{
    internal static class Scripts
    {
        public const string CreateStructureInitializer =
            @"IF NOT EXISTS (SELECT * FROM sys.schemas WHERE name = '{schema}')
            BEGIN
                EXEC('CREATE SCHEMA [{schema}]')
            END

            IF NOT EXISTS (SELECT * FROM sys.tables WHERE schema_id = SCHEMA_ID('{schema}') AND name = 'CustomerOrders')
            BEGIN
                CREATE TABLE [{schema}].[CustomerOrders] (
                    CorrelationId INT  NOT NULL PRIMARY KEY,
                    Json NVARCHAR(MAX) NOT NULL
                )
            END
            ";
    }
}
