namespace NemtPlatform.Domain.Entities.Fleet;

using NemtPlatform.Domain.Common;
using NemtPlatform.Domain.Common.Enums;

/// <summary>
/// Represents a physical equipment asset that can be assigned to vehicles.
/// Examples include wheelchairs, oxygen tanks, medical monitors, and other portable equipment.
/// </summary>
public class Equipment : TenantEntity
{
    /// <summary>
    /// Gets or sets the type of equipment.
    /// Required field describing what the equipment is (e.g., "Wheelchair", "Oxygen Tank", "Stretcher").
    /// </summary>
    public string EquipmentType { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the manufacturer's serial number for the equipment.
    /// Optional unique identifier for tracking and warranty purposes.
    /// </summary>
    public string? SerialNumber { get; set; }

    /// <summary>
    /// Gets or sets the operational status of the equipment.
    /// Indicates whether the equipment is available, in use, or under repair.
    /// </summary>
    public EquipmentStatus Status { get; set; } = EquipmentStatus.Available;

    /// <summary>
    /// Gets or sets the foreign key to the Vehicle the equipment is currently assigned to.
    /// Null if the equipment is not currently assigned to any vehicle.
    /// </summary>
    public string? AssignedVehicleId { get; set; }

    /// <summary>
    /// Gets or sets the date when the equipment last received service or maintenance.
    /// Optional field for tracking maintenance history.
    /// </summary>
    public DateTimeOffset? LastServiceDate { get; set; }

    /// <summary>
    /// Gets or sets the date when the equipment is scheduled for its next service.
    /// Optional field for planning preventative maintenance.
    /// </summary>
    public DateTimeOffset? NextServiceDate { get; set; }
}
