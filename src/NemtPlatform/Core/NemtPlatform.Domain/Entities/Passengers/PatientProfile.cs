namespace NemtPlatform.Domain.Entities.Passengers;

using NemtPlatform.Domain.Common;
using NemtPlatform.Domain.Common.Enums;
using NemtPlatform.Domain.Common.ValueObjects;

/// <summary>
/// A profile containing data specific to a passenger receiving medical transport (NEMT).
/// Contains medical, billing, and compliance information required for healthcare transportation.
/// </summary>
public class PatientProfile : TenantEntity
{
    /// <summary>
    /// Gets or sets the foreign key to the parent Passenger record.
    /// Required - every patient profile must be linked to a passenger.
    /// </summary>
    public string PassengerId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the URL to the patient's profile image.
    /// Used for driver identification and safety verification.
    /// </summary>
    public string? ImageUrl { get; set; }

    /// <summary>
    /// Gets or sets the patient's medical record number from their healthcare provider.
    /// Used for coordination with medical facilities.
    /// </summary>
    public string? MedicalRecordNumber { get; set; }

    /// <summary>
    /// Gets or sets the patient's mobility status.
    /// Determines the type of vehicle and equipment required for safe transport.
    /// </summary>
    public MobilityStatus? MobilityStatus { get; set; }

    /// <summary>
    /// Gets or sets special instructions for the driver and crew.
    /// Examples: "Oxygen required", "Fall risk", "Must use walker".
    /// </summary>
    public string? SpecialInstructions { get; set; }

    /// <summary>
    /// Gets or sets the foreign key to the default FundingSource for this patient.
    /// Used for billing and authorization when not specified at the trip level.
    /// </summary>
    public string? DefaultFundingSourceId { get; set; }

    /// <summary>
    /// Gets or sets the patient's Social Security Number.
    /// SENSITIVE DATA - Often required by Medicaid/Medicare for identity verification.
    /// Must be encrypted at rest.
    /// </summary>
    public string? SocialSecurityNumber { get; set; }

    /// <summary>
    /// Gets or sets the patient's Medicaid ID.
    /// Primary identifier for billing state Medicaid programs.
    /// </summary>
    public string? MedicaidId { get; set; }

    /// <summary>
    /// Gets or sets the verification metadata for the Medicaid ID.
    /// Tracks validation status and audit trail.
    /// </summary>
    public Verification? MedicaidIdVerification { get; set; }

    /// <summary>
    /// Gets or sets the patient's Medicare ID.
    /// Used for billing federal Medicare programs.
    /// </summary>
    public string? MedicareId { get; set; }

    /// <summary>
    /// Gets or sets the patient's Managed Care Organization (MCO) member ID.
    /// Used for billing managed care plans.
    /// </summary>
    public string? McoMemberId { get; set; }

    /// <summary>
    /// Gets or sets the patient's weight in pounds.
    /// Crucial for ensuring safety limits of vehicle lifts and for crew planning.
    /// </summary>
    public int? WeightInPounds { get; set; }

    /// <summary>
    /// Gets or sets the list of special communication needs.
    /// Informs crew how to best interact with the patient.
    /// </summary>
    public List<CommunicationNeed>? CommunicationNeeds { get; set; }

    /// <summary>
    /// Gets or sets the patient's cognitive status.
    /// Critical for drivers to understand patient's needs and potential wander risk.
    /// </summary>
    public CognitiveStatus? CognitiveStatus { get; set; }

    /// <summary>
    /// Gets or sets the list of required onboard medical equipment.
    /// Ensures correct vehicle assignment (e.g., with power inverter) and crew awareness.
    /// </summary>
    public List<OnboardEquipment>? RequiredOnboardEquipment { get; set; }

    /// <summary>
    /// Gets or sets whether the patient has given explicit consent to share medical information.
    /// Required for HIPAA compliance when coordinating with healthcare facilities.
    /// </summary>
    public bool ConsentToShareInfo { get; set; }

    /// <summary>
    /// Gets or sets the foreign key to the case manager (PartnerUser).
    /// Direct link to the coordinator for the patient's transport (e.g., at a hospital).
    /// </summary>
    public string? CaseManagerId { get; set; }
}
