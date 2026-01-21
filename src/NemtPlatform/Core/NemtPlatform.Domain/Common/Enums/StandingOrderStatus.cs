namespace NemtPlatform.Domain.Common.Enums;

/// <summary>
/// Defines the status of a recurring transportation standing order.
/// </summary>
public enum StandingOrderStatus
{
    /// <summary>
    /// Standing order is active and generating journeys.
    /// </summary>
    Active,

    /// <summary>
    /// Standing order is temporarily paused and not generating journeys.
    /// </summary>
    Paused,

    /// <summary>
    /// Standing order has ended and will no longer generate journeys.
    /// </summary>
    Ended
}
