namespace NemtPlatform.Application.Contracts.MultiTenancy;

/// <summary>
/// Provides access to the current tenant context.
/// Implemented by infrastructure to resolve tenant from HTTP headers, JWT claims, etc.
/// </summary>
public interface ITenantContext
{
    /// <summary>
    /// The ID of the current tenant. Null if no tenant context is available.
    /// </summary>
    string? TenantId { get; }

    /// <summary>
    /// Whether a valid tenant context exists.
    /// </summary>
    bool HasTenant => !string.IsNullOrEmpty(TenantId);
}
