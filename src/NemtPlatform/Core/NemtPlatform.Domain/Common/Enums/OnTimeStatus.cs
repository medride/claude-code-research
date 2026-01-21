namespace NemtPlatform.Domain.Common.Enums;

/// <summary>
/// Represents whether a trip or stop was completed on time relative to the scheduled time.
/// </summary>
public enum OnTimeStatus
{
    /// <summary>
    /// The trip or stop was completed within the acceptable time window.
    /// </summary>
    OnTime,

    /// <summary>
    /// The trip or stop was completed later than the acceptable time window.
    /// </summary>
    Late,

    /// <summary>
    /// The trip or stop was completed earlier than the scheduled time window.
    /// </summary>
    Early
}
