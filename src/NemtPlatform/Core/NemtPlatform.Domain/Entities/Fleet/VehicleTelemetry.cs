namespace NemtPlatform.Domain.Entities.Fleet;

using NemtPlatform.Domain.Common;
using NemtPlatform.Domain.Common.ValueObjects;

/// <summary>
/// Represents real-time telemetry data for a vehicle.
/// Lightweight entity designed for frequent updates of vehicle location and status.
/// Used for GPS tracking, route monitoring, and dispatch optimization.
/// </summary>
public class VehicleTelemetry : TenantEntity
{
    /// <summary>
    /// Gets or sets the foreign key to the Vehicle being tracked.
    /// Required link to the vehicle this telemetry data belongs to.
    /// </summary>
    public string VehicleId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the current GPS location of the vehicle.
    /// Required value object containing latitude and longitude coordinates.
    /// </summary>
    public GpsLocation Gps { get; set; } = null!;

    /// <summary>
    /// Gets or sets the timestamp when the telemetry data was last updated.
    /// Required field for determining data freshness and detecting stale locations.
    /// </summary>
    public DateTimeOffset LastUpdatedAt { get; set; }
}
