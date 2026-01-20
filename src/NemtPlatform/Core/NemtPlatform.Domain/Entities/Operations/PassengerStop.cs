namespace NemtPlatform.Domain.Entities.Operations;

using NemtPlatform.Domain.Common.Enums;
using NemtPlatform.Domain.Common.ValueObjects;

/// <summary>
/// Represents a stop where a passenger is picked up or dropped off.
/// Contains all information needed to execute the pickup/dropoff including location,
/// capacity changes, and procedural requirements.
/// </summary>
public class PassengerStop : BaseStop
{
    /// <summary>
    /// Gets or sets the type of passenger stop.
    /// Must be either Pickup or Dropoff. Other stop types use DriverServiceStop.
    /// </summary>
    public StopType Type { get; set; }

    /// <summary>
    /// Gets or sets the foreign key to the Passenger being picked up or dropped off.
    /// Null if the stop is associated with a companion or group but not a specific primary passenger.
    /// </summary>
    public string? PassengerId { get; set; }

    /// <summary>
    /// Gets or sets the foreign key to the AccessPoint where pickup/dropoff occurs.
    /// Required field. Specifies the exact entrance, door, or location within a place.
    /// </summary>
    public string AccessPointId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the foreign key to the Place (facility, residence, etc.).
    /// Required field. The broader location that contains the access point.
    /// </summary>
    public string PlaceId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the change in vehicle capacity at this stop.
    /// Positive values for pickups (adding passengers), negative for dropoffs (removing passengers).
    /// </summary>
    public CapacityRequirements CapacityDelta { get; set; } = new();

    /// <summary>
    /// Gets or sets procedure overrides that add or remove required driver tasks at this stop.
    /// Allows customization beyond default procedures set by funding source or trip configuration.
    /// Null if no overrides are needed.
    /// </summary>
    public ProcedureOverrides? ProcedureOverrides { get; set; }
}
