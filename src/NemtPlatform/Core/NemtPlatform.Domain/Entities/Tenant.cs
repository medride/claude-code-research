namespace NemtPlatform.Domain.Entities;

using NemtPlatform.Domain.Common;
using NemtPlatform.Domain.Common.ValueObjects;

/// <summary>
/// The master account for a company licensing the software.
/// All data in the system is partitioned by TenantId.
/// Note: This entity extends AuditableEntity (not TenantEntity) because
/// it IS the tenant - it doesn't belong to another tenant.
/// </summary>
public class Tenant : AuditableEntity
{
    /// <summary>
    /// The display name of the tenant organization.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// The operational status of the tenant account.
    /// </summary>
    public TenantStatus Status { get; set; } = TenantStatus.Trial;

    /// <summary>
    /// Primary contact information for the tenant.
    /// </summary>
    public TenantContact? PrimaryContact { get; set; }

    /// <summary>
    /// Physical address of the tenant organization.
    /// </summary>
    public Address? Address { get; set; }

    /// <summary>
    /// Tenant-specific configuration settings.
    /// </summary>
    public TenantSettings? Settings { get; set; }
}

/// <summary>
/// Represents the subscription/operational status of a tenant.
/// </summary>
public enum TenantStatus
{
    /// <summary>Trial period - limited features or time.</summary>
    Trial,

    /// <summary>Active paying customer.</summary>
    Active,

    /// <summary>Payment is overdue.</summary>
    PastDue,

    /// <summary>Account has been canceled.</summary>
    Canceled
}

/// <summary>
/// Contact information for a tenant's primary contact person.
/// </summary>
public record TenantContact(
    string Name,
    string Email,
    string Phone);

/// <summary>
/// Tenant-wide configuration settings.
/// </summary>
public record TenantSettings
{
    /// <summary>
    /// Regional settings like timezone and currency.
    /// </summary>
    public RegionalSettings? Regional { get; init; }

    /// <summary>
    /// Branding settings for white-labeling.
    /// </summary>
    public BrandingSettings? Branding { get; init; }

    /// <summary>
    /// Vehicle inspection requirements.
    /// </summary>
    public InspectionSettings? Inspections { get; init; }
}

/// <summary>
/// Regional configuration for a tenant.
/// </summary>
public record RegionalSettings(
    string Timezone,
    string Currency = "USD");

/// <summary>
/// Branding/white-label configuration for a tenant.
/// </summary>
public record BrandingSettings(
    string? LogoUrl = null,
    string? PrimaryColor = null);

/// <summary>
/// Vehicle inspection configuration for a tenant.
/// </summary>
public record InspectionSettings(
    bool RequirePreShiftInspection = true,
    bool RequirePostShiftInspection = false,
    string? DefaultPreShiftTemplateId = null,
    string? DefaultPostShiftTemplateId = null);
