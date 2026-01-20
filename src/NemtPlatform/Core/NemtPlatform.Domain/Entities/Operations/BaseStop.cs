namespace NemtPlatform.Domain.Entities.Operations;

using NemtPlatform.Domain.Common;
using NemtPlatform.Domain.Common.Enums;
using NemtPlatform.Domain.Common.ValueObjects;

/// <summary>
/// Abstract base class for all stop types in a transportation route.
/// Contains common properties for both passenger stops and driver service stops.
/// Extends TenantEntity to support multi-tenant isolation.
/// </summary>
public abstract class BaseStop : TenantEntity
{
    /// <summary>
    /// Gets or sets the current lifecycle status of the stop.
    /// Tracks the stop from initial planning through execution and completion.
    /// </summary>
    public StopStatus Status { get; set; }

    /// <summary>
    /// Gets or sets the estimated duration for this stop.
    /// Includes time for passenger boarding/alighting, driver procedures, or service activities.
    /// </summary>
    public TimeSpan Duration { get; set; }

    /// <summary>
    /// Gets or sets the foreign key to the Region this stop is located in.
    /// Used for regional routing and service area validation. Null if region is not tracked.
    /// </summary>
    public string? RegionId { get; set; }

    /// <summary>
    /// Gets or sets the list of time windows when this stop can occur.
    /// Multiple windows allow for flexible scheduling constraints.
    /// </summary>
    public List<TimeWindow> TimeWindows { get; set; } = new();

    /// <summary>
    /// Gets or sets operational notes or special instructions for the driver.
    /// May include gate codes, parking instructions, or other contextual information.
    /// </summary>
    public string? OperationalNotes { get; set; }

    /// <summary>
    /// Gets or sets the actual time the driver arrived at this stop.
    /// Null until the driver marks arrival. Used for performance tracking and billing.
    /// </summary>
    public DateTimeOffset? ActualArrivalTime { get; set; }

    /// <summary>
    /// Gets or sets the actual time the driver departed from this stop.
    /// Null until the driver marks departure. Used for performance tracking and billing.
    /// </summary>
    public DateTimeOffset? ActualDepartureTime { get; set; }
}
