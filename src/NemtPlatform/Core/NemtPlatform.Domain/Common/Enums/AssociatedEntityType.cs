namespace NemtPlatform.Domain.Common.Enums;

/// <summary>
/// Represents the type of entity that a document or record is associated with.
/// </summary>
public enum AssociatedEntityType
{
    /// <summary>
    /// Associated with a vehicle.
    /// </summary>
    Vehicle,

    /// <summary>
    /// Associated with a maintenance record.
    /// </summary>
    MaintenanceRecord,

    /// <summary>
    /// Associated with an employee.
    /// </summary>
    Employee,

    /// <summary>
    /// Associated with a trip.
    /// </summary>
    Trip
}
