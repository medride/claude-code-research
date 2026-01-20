namespace NemtPlatform.Domain.Common.Enums;

/// <summary>
/// Represents the real-time operational status of a trip currently in progress.
/// </summary>
public enum LiveStatus
{
    /// <summary>
    /// The trip has been dispatched to a driver.
    /// </summary>
    Dispatched,

    /// <summary>
    /// The driver is en route to the pickup location.
    /// </summary>
    EnRouteToPickup,

    /// <summary>
    /// The driver has arrived and is waiting at the pickup location.
    /// </summary>
    WaitingAtPickup,

    /// <summary>
    /// The passenger is on board and being transported to the destination.
    /// </summary>
    Transporting,

    /// <summary>
    /// The driver has arrived and is waiting at the dropoff location.
    /// </summary>
    WaitingAtDropoff
}
