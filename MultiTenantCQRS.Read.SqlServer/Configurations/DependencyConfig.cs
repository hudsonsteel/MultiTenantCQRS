using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MultiTenantCQRS.Read.Domain.Interfaces.Repositories;
using MultiTenantCQRS.Read.SqlServer.Repositories;
using MultiTenantCQRS.Read.SqlServer.Services;

namespace MultiTenantCQRS.Read.SqlServer.Configurations
{
    public static class DependencyConfig
    {
        public static void AddRepositoriesConfig(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<SqlServerOption>(configuration.GetSection("ConnectionStrings"));
            services.AddScoped<ISchemaRepository, SchemaRepository>();
            services.AddScoped<ServiceFactoryRepository>();
        }
    }
}
