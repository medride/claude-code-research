namespace NemtPlatform.Domain.Entities.Operations;

using NemtPlatform.Domain.Common.Enums;
using NemtPlatform.Domain.Common.ValueObjects;

/// <summary>
/// Represents a non-passenger stop for driver or vehicle service activities.
/// Used for breaks, refueling, maintenance, or waiting periods between trips.
/// </summary>
public class DriverServiceStop : BaseStop
{
    /// <summary>
    /// Gets or sets the type of service stop.
    /// Must be Break, Refuel, Maintenance, or Wait. Pickup/Dropoff use PassengerStop.
    /// </summary>
    public StopType Type { get; set; }

    /// <summary>
    /// Gets or sets the GPS coordinates where this service stop occurs.
    /// Null if the location is flexible or to be determined by the driver.
    /// </summary>
    public GpsLocation? Location { get; set; }
}
