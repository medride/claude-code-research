namespace NemtPlatform.Domain.Common.Enums;

/// <summary>
/// Represents the type of maintenance work being performed.
/// </summary>
public enum MaintenanceType
{
    /// <summary>
    /// Scheduled preventative maintenance to avoid future issues.
    /// </summary>
    Preventative,

    /// <summary>
    /// Repair work to fix an identified defect or issue.
    /// </summary>
    Repair,

    /// <summary>
    /// Vehicle inspection for safety or compliance.
    /// </summary>
    Inspection,

    /// <summary>
    /// Manufacturer-issued recall work.
    /// </summary>
    Recall
}
