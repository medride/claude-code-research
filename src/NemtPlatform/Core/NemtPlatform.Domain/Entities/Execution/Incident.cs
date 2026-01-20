namespace NemtPlatform.Domain.Entities.Execution;

using NemtPlatform.Domain.Common;
using NemtPlatform.Domain.Common.Enums;
using NemtPlatform.Domain.Common.ValueObjects;

/// <summary>
/// Represents an unplanned event that occurred during operations requiring documentation and response.
/// Incidents include accidents, breakdowns, medical emergencies, safety concerns, and service delays.
/// Critical for compliance, safety management, and operational improvement.
/// </summary>
public class Incident : TenantEntity
{
    /// <summary>
    /// Gets or sets the type of incident that occurred.
    /// Categorizes the incident for reporting and response procedures.
    /// </summary>
    public IncidentType Type { get; set; }

    /// <summary>
    /// Gets or sets the current status of the incident.
    /// Tracks the incident lifecycle from initial report through resolution and closure.
    /// </summary>
    public IncidentStatus Status { get; set; }

    /// <summary>
    /// Gets or sets the timestamp when the incident was first reported.
    /// Required field. Critical for timeline reconstruction and compliance reporting.
    /// </summary>
    public DateTimeOffset ReportedAt { get; set; }

    /// <summary>
    /// Gets or sets the GPS location where the incident occurred.
    /// Required field. Used for dispatch coordination and incident analysis.
    /// </summary>
    public GpsLocation Location { get; set; } = new(0, 0);

    /// <summary>
    /// Gets or sets the foreign key to the User who reported the incident.
    /// Required field. Typically the driver, but could be dispatch or other personnel.
    /// </summary>
    public string ReportedBy { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the foreign key to the Driver involved in the incident.
    /// Required field. Every incident must be associated with a driver.
    /// </summary>
    public string DriverId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the foreign key to the Vehicle involved in the incident.
    /// Required field. Every incident must be associated with a vehicle.
    /// </summary>
    public string VehicleId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the foreign key to the Route being executed when the incident occurred.
    /// Required field. Links the incident to the operational context.
    /// </summary>
    public string RouteId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the foreign key to the Stop being serviced when the incident occurred.
    /// Null if the incident occurred between stops or during dead-heading.
    /// </summary>
    public string? ActiveStopId { get; set; }

    /// <summary>
    /// Gets or sets the list of foreign keys to Passengers who were on board when the incident occurred.
    /// Important for passenger notifications, medical follow-up, and liability tracking.
    /// Empty list if no passengers were on board.
    /// </summary>
    public List<string> PassengerIdsOnBoard { get; set; } = new();

    /// <summary>
    /// Gets or sets the detailed description of what happened during the incident.
    /// Required field. Should include all relevant context for investigation and reporting.
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the list of actions taken in response to the incident.
    /// Null if no actions have been taken yet or the incident is still being assessed.
    /// </summary>
    public List<string>? ActionsTaken { get; set; }

    /// <summary>
    /// Gets or sets the notes describing how the incident was resolved.
    /// Null if the incident has not yet been resolved.
    /// </summary>
    public string? ResolutionNotes { get; set; }
}
