namespace NemtPlatform.Domain.Entities.Passengers;

using NemtPlatform.Domain.Common;
using NemtPlatform.Domain.Common.Enums;
using NemtPlatform.Domain.Common.ValueObjects;

/// <summary>
/// Core passenger record representing an individual who receives transportation services.
/// Can be a patient (NEMT) or student (school transport), or both.
/// A passenger might be created by a facility without requiring a login.
/// </summary>
public class Passenger : TenantEntity
{
    /// <summary>
    /// Gets or sets the foreign key to the Partner if this passenger was created by a partner organization.
    /// Null if created directly by the tenant.
    /// </summary>
    public string? PartnerId { get; set; }

    /// <summary>
    /// Gets or sets the foreign key to the User if this passenger has a login account.
    /// Null if the passenger was created without user authentication (e.g., by a facility).
    /// </summary>
    public string? UserId { get; set; }

    /// <summary>
    /// Gets or sets the passenger's name.
    /// Required field containing first and last name.
    /// </summary>
    public PersonName Name { get; set; } = null!;

    /// <summary>
    /// Gets or sets the passenger's phone number.
    /// Optional contact information.
    /// </summary>
    public string? PhoneNumber { get; set; }

    /// <summary>
    /// Gets or sets the passenger's date of birth.
    /// Used for age verification and eligibility determination.
    /// </summary>
    public DateTimeOffset? DateOfBirth { get; set; }

    /// <summary>
    /// Gets or sets the passenger's gender.
    /// May be used for matching constraints (e.g., same-gender driver requirements).
    /// </summary>
    public Gender? Gender { get; set; }

    /// <summary>
    /// Gets or sets the foreign key to the PatientProfile.
    /// If set, this passenger receives medical transport (NEMT) services.
    /// </summary>
    public string? PatientProfileId { get; set; }

    /// <summary>
    /// Gets or sets the foreign key to the StudentProfile.
    /// If set, this passenger receives school transport services.
    /// </summary>
    public string? StudentProfileId { get; set; }

    /// <summary>
    /// Gets or sets the passenger's permanent, default set of constraints.
    /// These constraints are applied to trips unless overridden at the trip level.
    /// </summary>
    public TripConstraints? Constraints { get; set; }

    /// <summary>
    /// Gets or sets the list of foreign keys to EmergencyContact entities.
    /// A passenger can have multiple emergency contacts.
    /// </summary>
    public List<string>? EmergencyContactIds { get; set; }
}
