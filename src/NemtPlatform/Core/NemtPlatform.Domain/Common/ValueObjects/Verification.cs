namespace NemtPlatform.Domain.Common.ValueObjects;

/// <summary>
/// Represents the verification status and metadata for data that requires validation.
/// Used to track data quality and audit verification processes.
/// </summary>
/// <param name="Status">The current verification status.</param>
/// <param name="VerifiedBy">The identifier of the person or system that verified the data. Null if not verified.</param>
/// <param name="VerificationMethod">The method or process used for verification (e.g., "Manual Review", "Automated Check"). Null if not verified.</param>
/// <param name="LastVerifiedAt">The timestamp when the data was last verified. Null if never verified.</param>
public record Verification(
    VerificationStatus Status,
    string? VerifiedBy = null,
    string? VerificationMethod = null,
    DateTimeOffset? LastVerifiedAt = null);

/// <summary>
/// Defines the possible verification states for data quality tracking.
/// </summary>
public enum VerificationStatus
{
    /// <summary>
    /// Data has not been verified yet.
    /// </summary>
    Unverified,

    /// <summary>
    /// Data has been verified and confirmed as correct.
    /// </summary>
    Verified,

    /// <summary>
    /// Data requires manual review before verification.
    /// </summary>
    NeedsReview,

    /// <summary>
    /// Data has been flagged as incorrect or suspicious.
    /// </summary>
    FlaggedIncorrect
}
