namespace NemtPlatform.Domain.Common.Enums;

/// <summary>
/// Represents the eligibility status of a patient for NEMT services.
/// </summary>
public enum EligibilityStatus
{
    /// <summary>
    /// Patient is currently eligible for services.
    /// </summary>
    Active,

    /// <summary>
    /// Patient is not currently eligible for services.
    /// </summary>
    Inactive,

    /// <summary>
    /// Patient eligibility is pending verification.
    /// </summary>
    PendingVerification
}
