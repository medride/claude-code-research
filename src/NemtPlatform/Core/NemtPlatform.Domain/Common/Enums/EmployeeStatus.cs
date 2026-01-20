namespace NemtPlatform.Domain.Common.Enums;

/// <summary>
/// Represents the employment status of an employee.
/// </summary>
public enum EmployeeStatus
{
    /// <summary>
    /// Employee is actively working.
    /// </summary>
    Active,

    /// <summary>
    /// Employee is temporarily on leave (medical, personal, etc.).
    /// </summary>
    OnLeave,

    /// <summary>
    /// Employee has been terminated or resigned.
    /// </summary>
    Terminated
}
