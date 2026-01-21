namespace NemtPlatform.Domain.Entities.Execution;

using NemtPlatform.Domain.Common;
using NemtPlatform.Domain.Common.Enums;
using NemtPlatform.Domain.Common.ValueObjects;

/// <summary>
/// Represents the real-time execution state of a trip as it progresses from dispatch through completion.
/// Tracks live status, actual times, route deviations, and stop reconciliations for billing and compliance.
/// </summary>
public class TripExecution : TenantEntity
{
    /// <summary>
    /// Gets or sets the foreign key to the Trip this execution record tracks.
    /// Required field. Every execution record must be associated with exactly one trip.
    /// </summary>
    public string TripId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the foreign key to the Route this trip is executing on.
    /// Required field. Links the trip to the driver's manifest for the day.
    /// </summary>
    public string RouteId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the current real-time operational status of the trip.
    /// Indicates where the driver is in the pickup-transport-dropoff lifecycle.
    /// </summary>
    public LiveStatus LiveStatus { get; set; }

    /// <summary>
    /// Gets or sets the approach route from the driver's current location to the pickup.
    /// Null if the driver has not yet started approaching or is already at the pickup.
    /// </summary>
    public DirectionsData? ApproachRoute { get; set; }

    /// <summary>
    /// Gets or sets the actual route being driven in real-time.
    /// May differ from the planned route due to traffic, detours, or driver decisions.
    /// Null if real-time tracking is not enabled or the trip has not started.
    /// </summary>
    public DirectionsData? LiveRoute { get; set; }

    /// <summary>
    /// Gets or sets whether the trip is running on time, late, or early.
    /// Null if timing has not yet been evaluated or the trip has not started.
    /// </summary>
    public OnTimeStatus? OnTimeStatus { get; set; }

    /// <summary>
    /// Gets or sets the actual timestamp when the driver started the trip (began approaching pickup).
    /// Null if the trip has not yet started.
    /// </summary>
    public DateTimeOffset? ActualStartTime { get; set; }

    /// <summary>
    /// Gets or sets the actual timestamp when the trip was completed (final dropoff).
    /// Null if the trip is still in progress or has not started.
    /// </summary>
    public DateTimeOffset? ActualEndTime { get; set; }

    /// <summary>
    /// Gets or sets the list of stop outcome reconciliations for billing and compliance.
    /// Contains the ground truth of what happened at each stop (pickup and dropoff).
    /// </summary>
    public List<StopReconciliation> Reconciliations { get; set; } = new();

    /// <summary>
    /// Gets or sets the reference identifier for an external electronic patient care report (ePCR) system.
    /// Used to link trip execution data with medical documentation systems.
    /// Null if no ePCR integration exists or no report was created.
    /// </summary>
    public string? EPcrReferenceId { get; set; }
}
