namespace NemtPlatform.Domain.Common.ValueObjects;

/// <summary>
/// Represents the verification status and metadata for data that requires validation.
/// Used to track data quality and audit verification processes.
/// </summary>
public record Verification
{
    /// <summary>
    /// The current verification status.
    /// </summary>
    public VerificationStatus Status { get; init; }

    /// <summary>
    /// The identifier of the person or system that verified the data. Null if not verified.
    /// </summary>
    public string? VerifiedBy { get; init; }

    /// <summary>
    /// The method or process used for verification (e.g., "Manual Review", "Automated Check"). Null if not verified.
    /// </summary>
    public string? VerificationMethod { get; init; }

    /// <summary>
    /// The timestamp when the data was last verified. Null if never verified.
    /// </summary>
    public DateTimeOffset? LastVerifiedAt { get; init; }

    /// <summary>
    /// Parameterless constructor for EF Core and JSON serialization.
    /// </summary>
    public Verification()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Verification"/> class.
    /// </summary>
    public Verification(
        VerificationStatus status,
        string? verifiedBy = null,
        string? verificationMethod = null,
        DateTimeOffset? lastVerifiedAt = null)
    {
        Status = status;
        VerifiedBy = verifiedBy;
        VerificationMethod = verificationMethod;
        LastVerifiedAt = lastVerifiedAt;
    }
}

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
