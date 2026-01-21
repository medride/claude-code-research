namespace NemtPlatform.Domain.Common.Enums;

/// <summary>
/// Defines pickup and drop-off safety restrictions for students.
/// Structured, critical safety data that is a key liability and safety feature.
/// </summary>
public enum PickupRestriction
{
    /// <summary>
    /// Guardian or authorized adult must be present at pickup/drop-off.
    /// Student cannot be released without adult supervision.
    /// </summary>
    GuardianMustBePresent,

    /// <summary>
    /// Student may be released to a sibling.
    /// </summary>
    SiblingCanReceive,

    /// <summary>
    /// Student may be released without adult supervision (self-release).
    /// </summary>
    SelfReleaseOk
}
