using Microsoft.Extensions.Options;
using MultiTenantCQRS.Read.Domain.Configurations;
using MultiTenantCQRS.Read.Domain.Interfaces.Repositories;
using MultiTenantCQRS.Read.SqlServer.Configurations;
using MultiTenantCQRS.Read.SqlServer.Scritps;
using System.Data;
using System.Data.SqlClient;

namespace MultiTenantCQRS.Read.SqlServer.Repositories
{
    public sealed class SchemaRepository(TenantOption tenantConfiguration, IOptions<SqlServerOption> sqlServerOption) : ISchemaRepository
    {
        private readonly TenantOption _tenantConfiguration = tenantConfiguration ?? throw new ArgumentNullException(nameof(tenantConfiguration));
        private readonly string _connectionString = sqlServerOption?.Value?.DefaultConnection ?? throw new ArgumentNullException(nameof(sqlServerOption));

        public async Task InitializeAsync(CancellationToken cancellationToken)
        {
            var script = Scripts.CreateStructureInitializer;

            foreach (var tenant in _tenantConfiguration.Tenants)
            {
                var schema = $"{tenant.SchemaName}_read";
                var scriptWithSchema = ReplaceSchemaPlaceholder(script, schema);

                try
                {
                    await ExecuteSqlScriptAsync(scriptWithSchema, cancellationToken);
                }
                catch (Exception ex)
                {
                    throw new ApplicationException($"Failed to execute script for schema {schema}", ex);
                }
            }
        }

        private static string ReplaceSchemaPlaceholder(string script, string schema)
        {
            ArgumentNullException.ThrowIfNull(script);
            ArgumentNullException.ThrowIfNull(schema);
            return script.Replace("{schema}", schema, StringComparison.OrdinalIgnoreCase);
        }

        private async Task ExecuteSqlScriptAsync(string script, CancellationToken cancellationToken)
        {
            await using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync(cancellationToken);

            await using var command = connection.CreateCommand();
            command.CommandText = script;
            command.CommandType = CommandType.Text;

            await command.ExecuteNonQueryAsync(cancellationToken);
        }
    }
}
