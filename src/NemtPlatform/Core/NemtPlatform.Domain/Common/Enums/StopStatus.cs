namespace NemtPlatform.Domain.Common.Enums;

/// <summary>
/// Represents the lifecycle of a stop, from initial planning through execution
/// and final completion or failure.
/// </summary>
public enum StopStatus
{
    /// <summary>
    /// The stop is scheduled but has not yet been dispatched to a driver's active route.
    /// </summary>
    Pending,

    /// <summary>
    /// The stop has been dispatched to a driver and is part of their active route.
    /// </summary>
    Assigned,

    /// <summary>
    /// The driver is currently en route to this stop.
    /// </summary>
    EnRoute,

    /// <summary>
    /// The driver has arrived at the stop location.
    /// </summary>
    Arrived,

    /// <summary>
    /// The stop has been successfully completed.
    /// </summary>
    Completed,

    /// <summary>
    /// The passenger did not appear at the scheduled stop.
    /// </summary>
    NoShow,

    /// <summary>
    /// The stop has been canceled.
    /// </summary>
    Canceled
}
