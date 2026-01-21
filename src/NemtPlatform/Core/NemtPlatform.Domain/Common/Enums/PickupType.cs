namespace NemtPlatform.Domain.Common.Enums;

/// <summary>
/// Defines the scheduling type for a trip pickup.
/// Determines whether the pickup occurs at a fixed scheduled time or on-demand.
/// </summary>
public enum PickupType
{
    /// <summary>
    /// The pickup occurs at a pre-scheduled, fixed time.
    /// The driver must arrive within the specified time window.
    /// </summary>
    Scheduled,

    /// <summary>
    /// The pickup is requested on-demand (will-call).
    /// The passenger will notify when they are ready for pickup.
    /// </summary>
    WillCall
}
