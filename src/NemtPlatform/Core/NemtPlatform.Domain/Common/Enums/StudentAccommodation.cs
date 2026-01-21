namespace NemtPlatform.Domain.Common.Enums;

/// <summary>
/// Defines special accommodations required for student transport.
/// Captures specific needs from an IEP or 504 Plan for compliance.
/// </summary>
public enum StudentAccommodation
{
    /// <summary>
    /// Vehicle must have a wheelchair lift.
    /// </summary>
    WheelchairLift,

    /// <summary>
    /// Student requires a safety harness or specialized restraint system.
    /// </summary>
    SafetyHarness,

    /// <summary>
    /// Student must be seated near the driver for supervision.
    /// </summary>
    SeatNearDriver,

    /// <summary>
    /// Student requires a behavioral aide to accompany them.
    /// </summary>
    BehavioralAideRequired
}
