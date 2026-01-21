namespace NemtPlatform.Domain.Common.Enums;

/// <summary>
/// Defines the high-level purpose of a stop.
/// This distinguishes between passenger-related activities and internal operational tasks.
/// </summary>
public enum StopType
{
    /// <summary>
    /// A passenger pickup stop.
    /// </summary>
    Pickup,

    /// <summary>
    /// A passenger dropoff stop.
    /// </summary>
    Dropoff,

    /// <summary>
    /// A driver break stop (meal, rest, etc.).
    /// </summary>
    Break,

    /// <summary>
    /// A refueling stop.
    /// </summary>
    Refuel,

    /// <summary>
    /// A vehicle maintenance stop.
    /// </summary>
    Maintenance,

    /// <summary>
    /// A waiting stop (e.g., waiting between appointments).
    /// </summary>
    Wait
}
