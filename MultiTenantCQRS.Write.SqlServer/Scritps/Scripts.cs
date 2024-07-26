namespace MultiTenantCQRS.Write.SqlServer.Scritps
{
    internal static class Scripts
    {
        public const string CreateStructureInitializer = @"
        -- Create schema if it does not exist
        IF NOT EXISTS (SELECT * FROM sys.schemas WHERE name = '{schema}')
        BEGIN
            EXEC('CREATE SCHEMA [{schema}]')
        END

        -- Create Customer table if it does not exist
        IF NOT EXISTS (SELECT * FROM sys.tables WHERE schema_id = SCHEMA_ID('{schema}') AND name = 'Customer')
        BEGIN
            CREATE TABLE [{schema}].[Customer] (
                Id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
                Name NVARCHAR(100) NOT NULL,
                Email NVARCHAR(100) NOT NULL
            )
        END

        -- Create Order table if it does not exist
        IF NOT EXISTS (SELECT * FROM sys.tables WHERE schema_id = SCHEMA_ID('{schema}') AND name = 'Order')
        BEGIN
            CREATE TABLE [{schema}].[Order] (
                Id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
                Date DATETIME2 NOT NULL,
                TotalAmount FLOAT NOT NULL,
                CustomerId INT NOT NULL,
                
                CONSTRAINT FK_Orders_Customer FOREIGN KEY (CustomerId) 
                    REFERENCES [{schema}].[Customer](Id) ON DELETE CASCADE
            )
        END
    ";
    }
}
