namespace NemtPlatform.Domain.Common.Enums;

/// <summary>
/// Defines medical equipment that must be onboard during transport.
/// Ensures correct vehicle assignment (e.g., with power inverter) and crew awareness.
/// </summary>
public enum OnboardEquipment
{
    /// <summary>
    /// Patient requires an oxygen concentrator during transport.
    /// Vehicle must have power inverter.
    /// </summary>
    OxygenConcentrator,

    /// <summary>
    /// Patient requires an IV pump during transport.
    /// Vehicle must have power inverter.
    /// </summary>
    IvPump,

    /// <summary>
    /// Patient requires a feeding pump during transport.
    /// Vehicle must have power inverter.
    /// </summary>
    FeedingPump
}
