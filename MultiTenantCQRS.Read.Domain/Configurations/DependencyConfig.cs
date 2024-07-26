using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MultiTenantCQRS.Read.Domain.Services;

namespace MultiTenantCQRS.Read.Domain.Configurations
{
    public static class DependencyConfig
    {
        public static void AddDomainConfig(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<TenantProvider>();
            services.Configure<TenantOption>(configuration.GetSection("TenantProviders"));
            services.AddSingleton(sp =>
            {
                var config = sp.GetRequiredService<IOptions<TenantOption>>().Value;
                return config;
            });
        }
    }
}
