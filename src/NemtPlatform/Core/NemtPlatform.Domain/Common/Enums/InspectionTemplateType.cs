namespace NemtPlatform.Domain.Common.Enums;

/// <summary>
/// Represents the type of inspection template.
/// </summary>
public enum InspectionTemplateType
{
    /// <summary>
    /// Inspection performed before starting a shift.
    /// </summary>
    PreShift,

    /// <summary>
    /// Inspection performed after completing a shift.
    /// </summary>
    PostShift
}
