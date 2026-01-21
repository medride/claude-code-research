namespace NemtPlatform.Domain.Entities.Operations;

using NemtPlatform.Domain.Common;
using NemtPlatform.Domain.Common.ValueObjects;

/// <summary>
/// Represents a driver's period of availability and their specific assignment.
/// It anchors the driver, vehicle, and time frame for a Route.
/// For NEMT, this may be a single driver. For ambulance services, this will be a multi-person crew.
/// </summary>
public class Shift : TenantEntity
{
    /// <summary>
    /// Gets or sets the vehicle assigned to operate for the duration of this shift.
    /// Required field.
    /// </summary>
    public string VehicleId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the list of personnel (crew) assigned to this shift.
    /// For NEMT, this may be a single driver.
    /// For ambulance services, this will be a multi-person crew.
    /// Required field.
    /// </summary>
    public List<ShiftPersonnel> Personnel { get; set; } = new();

    /// <summary>
    /// Gets or sets the scheduled start time for this specific shift instance.
    /// Required field.
    /// </summary>
    public DateTimeOffset StartTime { get; set; }

    /// <summary>
    /// Gets or sets the scheduled end time for this specific shift instance.
    /// Required field.
    /// </summary>
    public DateTimeOffset EndTime { get; set; }

    /// <summary>
    /// Gets or sets the designated starting location for the shift.
    /// Required field.
    /// </summary>
    public GpsLocation StartLocation { get; set; } = null!;

    /// <summary>
    /// Gets or sets the designated ending location for the shift.
    /// Required field.
    /// </summary>
    public GpsLocation EndLocation { get; set; } = null!;

    /// <summary>
    /// Gets or sets the pre-defined breaks or maintenance stops that must occur during the shift.
    /// This aligns with optimization and compliance models.
    /// Examples include meal breaks, refueling, or maintenance stops.
    /// Optional field.
    /// </summary>
    public List<DriverServiceStop>? RequiredStops { get; set; }
}
