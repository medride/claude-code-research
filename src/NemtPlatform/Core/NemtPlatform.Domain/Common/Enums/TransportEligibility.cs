namespace NemtPlatform.Domain.Common.Enums;

/// <summary>
/// Defines the eligibility reason for student transportation.
/// Often required for state funding reports and district-wide policies.
/// </summary>
public enum TransportEligibility
{
    /// <summary>
    /// Student is eligible based on distance from school.
    /// </summary>
    EligibleDistance,

    /// <summary>
    /// Student is eligible based on an Individualized Education Program (IEP).
    /// </summary>
    EligibleIep,

    /// <summary>
    /// Student is not eligible but family pays for service.
    /// </summary>
    IneligiblePaid,

    /// <summary>
    /// Other eligibility reason not covered by standard categories.
    /// </summary>
    Other
}
