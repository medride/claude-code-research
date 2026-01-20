namespace NemtPlatform.Domain.Common.Enums;

/// <summary>
/// Represents the status of a contract with a partner organization.
/// </summary>
public enum PartnerContractStatus
{
    /// <summary>
    /// Contract is in draft state and not yet finalized.
    /// </summary>
    Draft,

    /// <summary>
    /// Contract is active and in effect.
    /// </summary>
    Active,

    /// <summary>
    /// Contract has expired.
    /// </summary>
    Expired,

    /// <summary>
    /// Contract has been terminated.
    /// </summary>
    Terminated
}
