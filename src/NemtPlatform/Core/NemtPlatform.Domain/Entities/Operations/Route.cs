namespace NemtPlatform.Domain.Entities.Operations;

using NemtPlatform.Domain.Common;

/// <summary>
/// Represents the final, optimized sequence of stops assigned to a specific Shift for a given period.
/// This is the "plan of execution" or manifest that the crew must perform.
/// The stops list contains a mix of passenger stops and driver service stops (breaks, refueling, etc.).
/// </summary>
public class Route : TenantEntity
{
    /// <summary>
    /// Gets or sets the Shift that this Route is assigned to.
    /// This links to the crew and vehicle.
    /// Required field.
    /// </summary>
    public string ShiftId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the RouteManifest that this route was generated from.
    /// If this route was generated from a manifest, this links back to the source.
    /// Optional field.
    /// </summary>
    public string? SourceRouteManifestId { get; set; }

    /// <summary>
    /// Gets or sets the Incident that spawned this route.
    /// If this route was created in response to an incident, this links to it.
    /// Optional field.
    /// </summary>
    public string? SpawningIncidentId { get; set; }

    /// <summary>
    /// Gets or sets the final, ordered list of all stops (passenger, breaks, etc.) that the crew must perform.
    /// This is the "manifest" for the run.
    /// Contains a mix of PassengerStop and DriverServiceStop objects.
    /// Required field.
    /// </summary>
    public List<object> Stops { get; set; } = new();

    /// <summary>
    /// Gets or sets the estimated start time for the route.
    /// Required field.
    /// </summary>
    public DateTimeOffset EstimatedStartTime { get; set; }

    /// <summary>
    /// Gets or sets the estimated end time for the route.
    /// Required field.
    /// </summary>
    public DateTimeOffset EstimatedEndTime { get; set; }

    /// <summary>
    /// Gets or sets the estimated total distance for the route in meters.
    /// Required field.
    /// </summary>
    public int EstimatedTotalDistance { get; set; }

    /// <summary>
    /// Gets or sets the estimated duration for the route.
    /// Required field.
    /// </summary>
    public TimeSpan EstimatedDuration { get; set; }

    /// <summary>
    /// Gets or sets the actual start time for the route.
    /// Populated when the route execution begins.
    /// Optional field.
    /// </summary>
    public DateTimeOffset? ActualStartTime { get; set; }

    /// <summary>
    /// Gets or sets the actual end time for the route.
    /// Populated when the route execution completes.
    /// Optional field.
    /// </summary>
    public DateTimeOffset? ActualEndTime { get; set; }

    /// <summary>
    /// Gets or sets the actual total distance traveled in meters.
    /// Populated when the route execution completes.
    /// Optional field.
    /// </summary>
    public int? ActualTotalDistance { get; set; }

    /// <summary>
    /// Gets or sets the actual duration of the route.
    /// Populated when the route execution completes.
    /// Optional field.
    /// </summary>
    public TimeSpan? ActualDuration { get; set; }
}
