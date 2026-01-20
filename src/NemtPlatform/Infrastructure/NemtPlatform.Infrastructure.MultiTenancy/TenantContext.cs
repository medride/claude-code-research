using NemtPlatform.Application.Contracts.MultiTenancy;

namespace NemtPlatform.Infrastructure.MultiTenancy;

/// <summary>
/// Default implementation of ITenantContext.
/// Stores tenant ID for the current request scope.
/// </summary>
public class TenantContext : ITenantContext
{
    /// <summary>
    /// Gets or sets the ID of the current tenant. Null if no tenant context is available.
    /// </summary>
    public string? TenantId { get; set; }

    /// <summary>
    /// Gets a value indicating whether a valid tenant context exists.
    /// </summary>
    public bool HasTenant => !string.IsNullOrEmpty(TenantId);
}
