namespace NemtPlatform.Domain.Common.Enums;

/// <summary>
/// Represents the status of a patient's authorization for services.
/// </summary>
public enum AuthorizationStatus
{
    /// <summary>
    /// Authorization is currently active and valid.
    /// </summary>
    Active,

    /// <summary>
    /// Authorization has expired.
    /// </summary>
    Expired,

    /// <summary>
    /// Authorization has been fully utilized.
    /// </summary>
    Exhausted,

    /// <summary>
    /// Authorization has been canceled.
    /// </summary>
    Canceled
}
