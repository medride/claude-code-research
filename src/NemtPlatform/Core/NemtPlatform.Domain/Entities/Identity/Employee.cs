namespace NemtPlatform.Domain.Entities.Identity;

using NemtPlatform.Domain.Common;
using NemtPlatform.Domain.Common.Enums;
using NemtPlatform.Domain.Common.ValueObjects;

/// <summary>
/// HR record for staff members within a tenant organization.
/// This entity extends TenantEntity because employees belong to a specific tenant.
/// Links to the User entity for authentication and cross-tenant access.
/// </summary>
public class Employee : TenantEntity
{
    /// <summary>
    /// Gets or sets the foreign key to the User entity.
    /// Required link to the central authentication record.
    /// </summary>
    public string UserId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the current employment status.
    /// Determines whether the employee is actively working, on leave, or terminated.
    /// </summary>
    public EmployeeStatus Status { get; set; } = EmployeeStatus.Active;

    /// <summary>
    /// Gets or sets the date when the employee was hired.
    /// Used for calculating tenure and benefits eligibility.
    /// </summary>
    public DateTimeOffset HireDate { get; set; }

    /// <summary>
    /// Gets or sets the list of role identifiers assigned to this employee.
    /// Foreign keys to Role entities that define permissions and access levels.
    /// </summary>
    public List<string> RoleIds { get; set; } = new();

    /// <summary>
    /// Gets or sets the foreign key to the DriverProfile entity.
    /// Null if the employee is not a driver. Populated when the employee has driver-specific
    /// credentials, licenses, and vehicle assignments.
    /// </summary>
    public string? DriverProfileId { get; set; }

    /// <summary>
    /// Gets or sets the URL to the employee's profile image.
    /// Optional photo used for identification and staff directory.
    /// </summary>
    public string? ImageUrl { get; set; }

    /// <summary>
    /// Gets or sets the employee's date of birth.
    /// Optional field used for age verification and HR records.
    /// </summary>
    public DateTimeOffset? DateOfBirth { get; set; }

    /// <summary>
    /// Gets or sets the employee's physical mailing address.
    /// Optional value object containing street, city, state, and ZIP code.
    /// </summary>
    public Address? Address { get; set; }

    /// <summary>
    /// Gets or sets the list of emergency contact identifiers.
    /// Optional foreign keys to Contact entities for emergency situations.
    /// </summary>
    public List<string>? EmergencyContactIds { get; set; }
}
