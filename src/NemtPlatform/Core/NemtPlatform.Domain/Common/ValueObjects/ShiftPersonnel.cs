namespace NemtPlatform.Domain.Common.ValueObjects;

using NemtPlatform.Domain.Common.Enums;

/// <summary>
/// Represents a crew member assigned to a shift.
/// Includes their employee identifier and their role for the duration of the shift.
/// </summary>
/// <param name="EmployeeId">Foreign key to the Employee entity.</param>
/// <param name="Role">The operational role this employee performs during the shift.</param>
public record ShiftPersonnel(string EmployeeId, CrewRole Role);
