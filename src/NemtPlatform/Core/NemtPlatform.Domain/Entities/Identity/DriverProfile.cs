namespace NemtPlatform.Domain.Entities.Identity;

using NemtPlatform.Domain.Common;
using NemtPlatform.Domain.Common.Enums;
using NemtPlatform.Domain.Common.ValueObjects;

/// <summary>
/// Represents the operational profile for a driver.
/// Contains licensing information, compliance status, skills, and performance metrics.
/// Extends TenantEntity for multi-tenant support.
/// </summary>
public class DriverProfile : TenantEntity
{
    /// <summary>
    /// Gets or sets the foreign key to the parent Employee entity.
    /// </summary>
    public string EmployeeId { get; set; } = string.Empty;

    // Licensing & Compliance

    /// <summary>
    /// Gets or sets the driver's license number.
    /// </summary>
    public string DriversLicenseNumber { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the license class (e.g., "C", "CDL-B").
    /// </summary>
    public string? LicenseClass { get; set; }

    /// <summary>
    /// Gets or sets the list of license endorsements (e.g., "P" for Passenger, "S" for School Bus).
    /// </summary>
    public List<string>? LicenseEndorsements { get; set; }

    /// <summary>
    /// Gets or sets the state or province that issued the license (e.g., "CA", "NY").
    /// </summary>
    public string? LicenseState { get; set; }

    /// <summary>
    /// Gets or sets the current compliance status for the driver.
    /// </summary>
    public ComplianceStatus CurrentComplianceStatus { get; set; } = ComplianceStatus.Clear;

    // Skills

    /// <summary>
    /// Gets or sets the list of driver skills with proficiency levels.
    /// </summary>
    public List<DriverSkill>? Skills { get; set; }

    // Performance

    /// <summary>
    /// Gets or sets the driver's performance metrics including on-time percentage,
    /// passenger satisfaction rating, and incident-free miles.
    /// </summary>
    public DriverPerformanceMetrics? PerformanceMetrics { get; set; }
}
