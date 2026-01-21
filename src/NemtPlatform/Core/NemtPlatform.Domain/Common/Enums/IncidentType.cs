namespace NemtPlatform.Domain.Common.Enums;

/// <summary>
/// Represents the type of incident that occurred during a trip or operation.
/// </summary>
public enum IncidentType
{
    /// <summary>
    /// Vehicle was involved in an accident.
    /// </summary>
    VehicleAccident,

    /// <summary>
    /// Vehicle experienced a mechanical breakdown.
    /// </summary>
    VehicleBreakdown,

    /// <summary>
    /// Passenger experienced a medical emergency.
    /// </summary>
    PassengerMedicalEmergency,

    /// <summary>
    /// Driver experienced a medical emergency.
    /// </summary>
    DriverMedicalEmergency,

    /// <summary>
    /// Safety concern was identified.
    /// </summary>
    SafetyConcern,

    /// <summary>
    /// Service was delayed beyond acceptable parameters.
    /// </summary>
    ServiceDelay
}
