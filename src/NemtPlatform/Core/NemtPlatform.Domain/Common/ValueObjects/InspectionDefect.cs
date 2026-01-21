namespace NemtPlatform.Domain.Common.ValueObjects;

/// <summary>
/// Represents a defect or issue found during a vehicle inspection.
/// </summary>
public record InspectionDefect
{
    /// <summary>
    /// The category or system where the defect was found (e.g., "Brakes", "Lights", "Tires").
    /// </summary>
    public string Category { get; init; }

    /// <summary>
    /// Detailed description of the defect or issue.
    /// </summary>
    public string Description { get; init; }

    /// <summary>
    /// Whether this defect is critical and requires immediate attention before the vehicle can be used.
    /// </summary>
    public bool IsCritical { get; init; }

    /// <summary>
    /// Parameterless constructor for EF Core and JSON serialization.
    /// </summary>
    public InspectionDefect()
    {
        Category = string.Empty;
        Description = string.Empty;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="InspectionDefect"/> class.
    /// </summary>
    public InspectionDefect(string category, string description, bool isCritical)
    {
        Category = category;
        Description = description;
        IsCritical = isCritical;
    }
}
