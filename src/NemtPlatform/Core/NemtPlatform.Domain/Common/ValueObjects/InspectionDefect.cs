namespace NemtPlatform.Domain.Common.ValueObjects;

/// <summary>
/// Represents a defect or issue found during a vehicle inspection.
/// </summary>
/// <param name="Category">The category or system where the defect was found (e.g., "Brakes", "Lights", "Tires").</param>
/// <param name="Description">Detailed description of the defect or issue.</param>
/// <param name="IsCritical">Whether this defect is critical and requires immediate attention before the vehicle can be used.</param>
public record InspectionDefect(
    string Category,
    string Description,
    bool IsCritical);
