namespace NemtPlatform.Domain.Entities.Fleet;

using NemtPlatform.Domain.Common;
using NemtPlatform.Domain.Common.Enums;
using NemtPlatform.Domain.Common.ValueObjects;

/// <summary>
/// Represents a maintenance work order for a vehicle.
/// Tracks preventative maintenance, repairs, inspections, and recalls.
/// </summary>
public class MaintenanceRecord : TenantEntity
{
    /// <summary>
    /// Gets or sets the foreign key to the Vehicle being serviced.
    /// Required link to the vehicle receiving maintenance.
    /// </summary>
    public string VehicleId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the status of the work order.
    /// Tracks progression from requested through scheduled, in progress, to completed or canceled.
    /// </summary>
    public WorkOrderStatus Status { get; set; } = WorkOrderStatus.Requested;

    /// <summary>
    /// Gets or sets the type of maintenance being performed.
    /// Indicates whether this is preventative, repair, inspection, or recall work.
    /// </summary>
    public MaintenanceType Type { get; set; }

    /// <summary>
    /// Gets or sets the description of the maintenance work.
    /// Required field describing what work is being performed and why.
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the odometer reading at the time of service.
    /// Used for tracking mileage-based maintenance schedules.
    /// </summary>
    public int OdometerReading { get; set; }

    /// <summary>
    /// Gets or sets the cost breakdown for the maintenance work.
    /// Optional value object containing parts, labor, tax, and total costs.
    /// </summary>
    public MaintenanceCosts? Costs { get; set; }

    /// <summary>
    /// Gets or sets the date when the maintenance was requested.
    /// Optional field for tracking when the work order was initiated.
    /// </summary>
    public DateTimeOffset? DateRequested { get; set; }

    /// <summary>
    /// Gets or sets the scheduled date for the maintenance work.
    /// Optional field for planning and resource allocation.
    /// </summary>
    public DateTimeOffset? ScheduledDate { get; set; }

    /// <summary>
    /// Gets or sets the date when the maintenance was completed.
    /// Optional field populated when work is finished.
    /// </summary>
    public DateTimeOffset? CompletionDate { get; set; }

    /// <summary>
    /// Gets or sets the foreign key to an external vendor or service provider.
    /// Optional identifier if work was performed by a third-party vendor.
    /// </summary>
    public string? ExternalVendorId { get; set; }

    /// <summary>
    /// Gets or sets the list of employee identifiers for internal mechanics who performed the work.
    /// Optional list of foreign keys to Employee entities.
    /// </summary>
    public List<string>? InternalMechanicIds { get; set; }

    /// <summary>
    /// Gets or sets the foreign key to the VehicleInspection that identified the need for this work.
    /// Optional link if maintenance was triggered by an inspection defect.
    /// </summary>
    public string? SourceInspectionId { get; set; }

    /// <summary>
    /// Gets or sets the foreign key to an Incident that triggered this maintenance work.
    /// Optional link if work is needed due to an accident or incident.
    /// </summary>
    public string? SourceIncidentId { get; set; }

    /// <summary>
    /// Gets or sets additional notes about the maintenance work.
    /// Optional free-text field for mechanic observations or special instructions.
    /// </summary>
    public string? Notes { get; set; }
}
