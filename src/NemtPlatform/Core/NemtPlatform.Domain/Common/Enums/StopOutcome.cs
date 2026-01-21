namespace NemtPlatform.Domain.Common.Enums;

/// <summary>
/// Describes the final result of a stop after it has been attempted.
/// This is crucial for billing, reporting, and payroll.
/// </summary>
public enum StopOutcome
{
    /// <summary>
    /// The stop was completed exactly as planned with no issues.
    /// </summary>
    CompletedAsPlanned,

    /// <summary>
    /// The stop was completed but with some variance from the plan (e.g., guest no-show).
    /// </summary>
    CompletedWithVariance,

    /// <summary>
    /// The passenger did not show up for the scheduled stop.
    /// </summary>
    PassengerNoShow,

    /// <summary>
    /// The stop was canceled when the driver arrived at the location.
    /// </summary>
    CanceledAtDoor,

    /// <summary>
    /// The stop could not be completed due to vehicle breakdown.
    /// </summary>
    VehicleBrokeDown
}
