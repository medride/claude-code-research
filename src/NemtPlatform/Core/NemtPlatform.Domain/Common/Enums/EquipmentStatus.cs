namespace NemtPlatform.Domain.Common.Enums;

/// <summary>
/// Represents the operational status of vehicle equipment.
/// </summary>
public enum EquipmentStatus
{
    /// <summary>
    /// Equipment is available and not currently assigned to a vehicle.
    /// </summary>
    Available,

    /// <summary>
    /// Equipment is currently in use and assigned to a vehicle.
    /// </summary>
    InUse,

    /// <summary>
    /// Equipment is undergoing repair or maintenance.
    /// </summary>
    InRepair
}
