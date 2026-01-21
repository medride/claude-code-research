namespace NemtPlatform.Domain.Common.ValueObjects;

using NemtPlatform.Domain.Common.Enums;

/// <summary>
/// Represents a crew member assigned to a shift.
/// Includes their employee identifier and their role for the duration of the shift.
/// </summary>
public record ShiftPersonnel
{
    /// <summary>
    /// Foreign key to the Employee entity.
    /// </summary>
    public string EmployeeId { get; init; }

    /// <summary>
    /// The operational role this employee performs during the shift.
    /// </summary>
    public CrewRole Role { get; init; }

    /// <summary>
    /// Parameterless constructor for EF Core and JSON serialization.
    /// </summary>
    public ShiftPersonnel()
    {
        EmployeeId = string.Empty;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ShiftPersonnel"/> class.
    /// </summary>
    public ShiftPersonnel(string employeeId, CrewRole role)
    {
        EmployeeId = employeeId;
        Role = role;
    }
}
