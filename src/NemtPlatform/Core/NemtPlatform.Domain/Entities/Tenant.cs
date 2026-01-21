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
public record TenantContact
{
    /// <summary>Name of the contact person.</summary>
    public string Name { get; init; } = string.Empty;

    /// <summary>Email address of the contact.</summary>
    public string Email { get; init; } = string.Empty;

    /// <summary>Phone number of the contact.</summary>
    public string Phone { get; init; } = string.Empty;

    /// <summary>Parameterless constructor for EF Core and JSON serialization.</summary>
    public TenantContact() { }

    /// <summary>Creates a new TenantContact with the specified values.</summary>
    public TenantContact(string name, string email, string phone)
    {
        Name = name;
        Email = email;
        Phone = phone;
    }
}

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

    /// <summary>Parameterless constructor for EF Core and JSON serialization.</summary>
    public TenantSettings() { }
}

/// <summary>
/// Regional configuration for a tenant.
/// </summary>
public record RegionalSettings
{
    /// <summary>IANA timezone identifier (e.g., "America/New_York").</summary>
    public string Timezone { get; init; } = string.Empty;

    /// <summary>ISO 4217 currency code (e.g., "USD").</summary>
    public string Currency { get; init; } = "USD";

    /// <summary>Parameterless constructor for EF Core and JSON serialization.</summary>
    public RegionalSettings() { }

    /// <summary>Creates a new RegionalSettings with the specified values.</summary>
    public RegionalSettings(string timezone, string currency = "USD")
    {
        Timezone = timezone;
        Currency = currency;
    }
}

/// <summary>
/// Branding/white-label configuration for a tenant.
/// </summary>
public record BrandingSettings
{
    /// <summary>URL to the tenant's logo image.</summary>
    public string? LogoUrl { get; init; }

    /// <summary>Primary brand color in hex format (e.g., "#FF5733").</summary>
    public string? PrimaryColor { get; init; }

    /// <summary>Parameterless constructor for EF Core and JSON serialization.</summary>
    public BrandingSettings() { }

    /// <summary>Creates a new BrandingSettings with the specified values.</summary>
    public BrandingSettings(string? logoUrl = null, string? primaryColor = null)
    {
        LogoUrl = logoUrl;
        PrimaryColor = primaryColor;
    }
}

/// <summary>
/// Vehicle inspection configuration for a tenant.
/// </summary>
public record InspectionSettings
{
    /// <summary>Whether drivers must complete an inspection before starting a shift.</summary>
    public bool RequirePreShiftInspection { get; init; } = true;

    /// <summary>Whether drivers must complete an inspection after ending a shift.</summary>
    public bool RequirePostShiftInspection { get; init; }

    /// <summary>Default inspection template ID for pre-shift inspections.</summary>
    public string? DefaultPreShiftTemplateId { get; init; }

    /// <summary>Default inspection template ID for post-shift inspections.</summary>
    public string? DefaultPostShiftTemplateId { get; init; }

    /// <summary>Parameterless constructor for EF Core and JSON serialization.</summary>
    public InspectionSettings() { }

    /// <summary>Creates a new InspectionSettings with the specified values.</summary>
    public InspectionSettings(
        bool requirePreShiftInspection = true,
        bool requirePostShiftInspection = false,
        string? defaultPreShiftTemplateId = null,
        string? defaultPostShiftTemplateId = null)
    {
        RequirePreShiftInspection = requirePreShiftInspection;
        RequirePostShiftInspection = requirePostShiftInspection;
        DefaultPreShiftTemplateId = defaultPreShiftTemplateId;
        DefaultPostShiftTemplateId = defaultPostShiftTemplateId;
    }
}
