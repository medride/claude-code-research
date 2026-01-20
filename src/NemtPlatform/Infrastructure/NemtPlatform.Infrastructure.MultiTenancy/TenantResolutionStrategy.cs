namespace NemtPlatform.Infrastructure.MultiTenancy;

/// <summary>
/// Defines the supported strategies for resolving tenant identification from incoming requests.
/// </summary>
public enum TenantResolutionStrategy
{
    /// <summary>
    /// Resolve tenant ID from the X-Tenant-Id HTTP header.
    /// </summary>
    Header,

    /// <summary>
    /// Resolve tenant ID from the subdomain (e.g., tenant.example.com).
    /// </summary>
    Subdomain,

    /// <summary>
    /// Resolve tenant ID from the URL path (e.g., /tenant-id/api/...).
    /// </summary>
    Path,

    /// <summary>
    /// Resolve tenant ID from a JWT claim.
    /// </summary>
    Claim
}
