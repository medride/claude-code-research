namespace NemtPlatform.Domain.Entities.Operations;

using NemtPlatform.Domain.Common;
using NemtPlatform.Domain.Common.Enums;

/// <summary>
/// Represents a collection of individual Trips that are grouped together to be performed
/// by a single vehicle in a single run.
/// This is the core entity for managing multi-passenger services like school buses or shuttles.
/// </summary>
public class RouteManifest : TenantEntity
{
    /// <summary>
    /// Gets or sets the name of the route manifest.
    /// For example: "Route 101 AM", "Airport Shuttle Loop".
    /// Required field.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the list of individual Trip IDs that constitute this manifest.
    /// Required field.
    /// </summary>
    public List<string> TripIds { get; set; } = new();

    /// <summary>
    /// Gets or sets the single, overarching optimization job ID generated from this manifest.
    /// This is populated by the adapter before sending to the optimizer.
    /// Optional field.
    /// </summary>
    public string? OptimizationJobId { get; set; }

    /// <summary>
    /// Gets or sets the status of the route manifest in the planning and execution lifecycle.
    /// Required field.
    /// </summary>
    public RouteManifestStatus Status { get; set; }

    /// <summary>
    /// Gets or sets the Shift (driver/vehicle) this manifest is assigned to.
    /// Optional field.
    /// </summary>
    public string? ShiftId { get; set; }
}
