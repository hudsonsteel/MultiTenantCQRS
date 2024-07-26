using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using MultiTenantCQRS.Read.Domain.Configurations;

namespace MultiTenantCQRS.Read.Domain.Services
{
    public sealed class TenantProvider(IHttpContextAccessor httpContextAccessor, IOptions<TenantOption> tenantConfiguration)
    {
        private const string TenantIdHeaderName = "X-TenantId";
        private const int DefaultTenantId = -1;
        private int _tenantId = DefaultTenantId;

        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        private readonly TenantOption _tenantConfiguration = tenantConfiguration?.Value ?? throw new ArgumentNullException(nameof(tenantConfiguration));

        public void SetTenantId(int tenantId)
        {
            _tenantId = tenantId;
        }

        public int GetTenantId()
        {
            if (_httpContextAccessor.HttpContext == null)
            {
                return _tenantId != DefaultTenantId ? _tenantId : throw new InvalidOperationException("HttpContext is not available and TenantId is not set.");
            }

            var tenantIdHeader = _httpContextAccessor.HttpContext.Request.Headers[TenantIdHeaderName];
            if (!int.TryParse(tenantIdHeader, out var tenantId) || !IsValidTenantId(tenantId))
            {
                return _tenantId != DefaultTenantId ? _tenantId : throw new InvalidOperationException("Invalid Tenant ID from HttpContext and TenantId is not set.");
            }

            return _tenantId <= DefaultTenantId ? _tenantId : tenantId;
        }

        public string GetSchemaName()
        {
            var tenantId = GetTenantId();

            if (tenantId == DefaultTenantId)
            {
                throw new InvalidOperationException("Invalid Tenant ID.");
            }

            var tenant = _tenantConfiguration.Tenants.FirstOrDefault(t => t.TenantId == tenantId);

            return tenant == null ? throw new InvalidOperationException($"Schema not found for Tenant ID {tenantId}.") : tenant.SchemaName;
        }

        private bool IsValidTenantId(int tenantId)
        {
            return _tenantConfiguration.Tenants.Any(t => t.TenantId == tenantId);
        }
    }
}
