namespace NemtPlatform.Domain.Common.Enums;

/// <summary>
/// Represents the outcome of a vehicle inspection.
/// </summary>
public enum InspectionStatus
{
    /// <summary>
    /// Vehicle passed inspection with no issues.
    /// </summary>
    Pass,

    /// <summary>
    /// Vehicle passed inspection with minor notes or observations.
    /// </summary>
    PassWithNotes,

    /// <summary>
    /// Vehicle failed inspection and requires corrective action.
    /// </summary>
    Fail
}
