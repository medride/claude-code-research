namespace NemtPlatform.Domain.Common.Enums;

/// <summary>
/// Represents the status of a credential or certification.
/// </summary>
public enum CredentialStatus
{
    /// <summary>
    /// Credential is pending verification.
    /// </summary>
    PendingVerification,

    /// <summary>
    /// Credential is active and valid.
    /// </summary>
    Active,

    /// <summary>
    /// Credential has expired.
    /// </summary>
    Expired,

    /// <summary>
    /// Credential has been revoked.
    /// </summary>
    Revoked
}
