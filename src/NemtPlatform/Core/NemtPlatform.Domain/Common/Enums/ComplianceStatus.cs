namespace NemtPlatform.Domain.Common.Enums;

/// <summary>
/// Represents the compliance status for regulatory requirements.
/// </summary>
public enum ComplianceStatus
{
    /// <summary>
    /// All compliance requirements are met.
    /// </summary>
    Clear,

    /// <summary>
    /// Compliance certification or documentation needs renewal.
    /// </summary>
    NeedsRenewal,

    /// <summary>
    /// Compliance status is suspended pending resolution.
    /// </summary>
    Suspended
}
