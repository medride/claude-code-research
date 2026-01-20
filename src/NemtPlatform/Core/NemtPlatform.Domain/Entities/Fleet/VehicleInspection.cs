namespace NemtPlatform.Domain.Entities.Fleet;

using NemtPlatform.Domain.Common;
using NemtPlatform.Domain.Common.Enums;
using NemtPlatform.Domain.Common.ValueObjects;

/// <summary>
/// Represents a vehicle inspection performed by a driver.
/// Typically conducted at the start or end of a shift to ensure vehicle safety and compliance.
/// </summary>
public class VehicleInspection : TenantEntity
{
    /// <summary>
    /// Gets or sets the foreign key to the Vehicle being inspected.
    /// Required link to the vehicle that was inspected.
    /// </summary>
    public string VehicleId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the foreign key to the Driver who performed the inspection.
    /// Required link to the employee who conducted the inspection.
    /// </summary>
    public string DriverId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the foreign key to the Shift during which the inspection was performed.
    /// Optional link to the associated shift for tracking and reporting.
    /// </summary>
    public string? ShiftId { get; set; }

    /// <summary>
    /// Gets or sets the outcome of the inspection.
    /// Indicates whether the vehicle passed, passed with notes, or failed the inspection.
    /// </summary>
    public InspectionStatus Status { get; set; }

    /// <summary>
    /// Gets or sets the odometer reading at the time of inspection.
    /// Used for tracking vehicle usage and maintenance schedules.
    /// </summary>
    public int OdometerReading { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the inspection was performed.
    /// Required timestamp for audit trail and compliance reporting.
    /// </summary>
    public DateTimeOffset InspectionDate { get; set; }

    /// <summary>
    /// Gets or sets the list of defects found during the inspection.
    /// Optional list of inspection defect value objects detailing any issues discovered.
    /// </summary>
    public List<InspectionDefect>? DefectsFound { get; set; }

    /// <summary>
    /// Gets or sets the URL to the driver's digital signature.
    /// Optional link to signature image confirming the driver completed the inspection.
    /// </summary>
    public string? DriverSignatureUrl { get; set; }

    /// <summary>
    /// Gets or sets additional notes from the inspection.
    /// Optional free-text field for observations not captured in defect categories.
    /// </summary>
    public string? Notes { get; set; }
}
