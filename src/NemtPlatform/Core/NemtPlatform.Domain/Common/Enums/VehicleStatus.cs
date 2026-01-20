namespace NemtPlatform.Domain.Common.Enums;

/// <summary>
/// Represents the operational status of a vehicle.
/// </summary>
public enum VehicleStatus
{
    /// <summary>
    /// Vehicle is active and available for service.
    /// </summary>
    Active,

    /// <summary>
    /// Vehicle is inactive and not available for service.
    /// </summary>
    Inactive,

    /// <summary>
    /// Vehicle is undergoing maintenance or repairs.
    /// </summary>
    InMaintenance,

    /// <summary>
    /// Vehicle has been permanently removed from service.
    /// </summary>
    Decommissioned
}
