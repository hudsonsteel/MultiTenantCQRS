using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MultiTenantCQRS.Write.Domain.Interfaces.Repositories;
using MultiTenantCQRS.Write.SqlServer.Repositories;
using MultiTenantCQRS.Write.SqlServer.Services;

namespace MultiTenantCQRS.Write.SqlServer.Configurations
{
    public static class DependencyConfig
    {
        public static void AddRepositoriesConfig(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<SqlServerOption>(configuration.GetSection("SqlServerOption"));

            services.AddScoped<ISchemaRepository, SchemaRepository>();
            services.Configure<SqlServerOption>(configuration.GetSection("ConnectionStrings"));
            services.AddScoped<ServiceFactoryRepository>();
        }
    }
}
