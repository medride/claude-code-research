namespace NemtPlatform.Domain.Common.Enums;

/// <summary>
/// Represents the status of a claim for reimbursement.
/// </summary>
public enum ClaimStatus
{
    /// <summary>
    /// Claim has been created but not yet submitted.
    /// </summary>
    Draft,

    /// <summary>
    /// Claim has been submitted for processing.
    /// </summary>
    Submitted,

    /// <summary>
    /// Claim has been fully paid.
    /// </summary>
    Paid,

    /// <summary>
    /// Claim has been partially paid.
    /// </summary>
    PartiallyPaid,

    /// <summary>
    /// Claim has been denied.
    /// </summary>
    Denied,

    /// <summary>
    /// Claim is pending additional information.
    /// </summary>
    PendingInfo
}
