namespace NemtPlatform.Domain.Entities.Passengers;

using NemtPlatform.Domain.Common;
using NemtPlatform.Domain.Common.Enums;

/// <summary>
/// A profile containing data specific to a student passenger receiving school transportation.
/// Contains educational, safety, and compliance information required for student transport services.
/// </summary>
public class StudentProfile : TenantEntity
{
    /// <summary>
    /// Gets or sets the foreign key to the parent Passenger record.
    /// Required - every student profile must be linked to a passenger.
    /// </summary>
    public string PassengerId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the URL to the student's profile image.
    /// Used for driver identification and safety verification.
    /// </summary>
    public string? ImageUrl { get; set; }

    /// <summary>
    /// Gets or sets the student's ID number from their school.
    /// Used for coordination with educational institutions.
    /// </summary>
    public string? StudentIdNumber { get; set; }

    /// <summary>
    /// Gets or sets the foreign key to the School entity.
    /// Links the student to their educational institution.
    /// </summary>
    public string? SchoolId { get; set; }

    /// <summary>
    /// Gets or sets the student's current grade level.
    /// Used for age-appropriate routing and reporting.
    /// </summary>
    public int? GradeLevel { get; set; }

    /// <summary>
    /// Gets or sets special instructions for dismissal procedures.
    /// Example: "Must be met by guardian at bus stop."
    /// </summary>
    public string? DismissalInstructions { get; set; }

    /// <summary>
    /// Gets or sets the foreign key to the SchoolDistrict entity.
    /// Essential for reporting, billing, and associating with district-wide policies.
    /// </summary>
    public string? SchoolDistrictId { get; set; }

    /// <summary>
    /// Gets or sets the reason for transportation eligibility.
    /// Tracks the reason for eligibility, often required for state funding reports.
    /// </summary>
    public TransportEligibility? TransportationEligibility { get; set; }

    /// <summary>
    /// Gets or sets the foreign key to the assigned RouteManifest.
    /// The student's default, recurring route for daily manifest generation.
    /// </summary>
    public string? AssignedRouteId { get; set; }

    /// <summary>
    /// Gets or sets the foreign key to the assigned BusStop.
    /// Links the student to their designated pickup/drop-off spot.
    /// </summary>
    public string? AssignedStopId { get; set; }

    /// <summary>
    /// Gets or sets the list of pickup and drop-off safety restrictions.
    /// Structured, critical safety data that is a key liability and safety feature.
    /// Replaces free-text dismissal instructions with enforceable policies.
    /// </summary>
    public List<PickupRestriction>? PickupDropoffRestrictions { get; set; }

    /// <summary>
    /// Gets or sets the list of required accommodations for transport.
    /// Captures specific needs from an IEP or 504 Plan for compliance.
    /// Ensures the correct vehicle and crew are assigned.
    /// </summary>
    public List<StudentAccommodation>? RequiredAccommodations { get; set; }

    /// <summary>
    /// Gets or sets relevant allergy information.
    /// Critical medical info available to driver/aides in an emergency.
    /// Example: "Severe peanut allergy, carries EpiPen"
    /// </summary>
    public string? RelevantAllergies { get; set; }

    /// <summary>
    /// Gets or sets confidential behavioral notes.
    /// Need-to-know information for drivers and aides to de-escalate situations.
    /// Should be handled with appropriate access controls.
    /// </summary>
    public string? BehavioralNotes { get; set; }
}
