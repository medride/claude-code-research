namespace NemtPlatform.Domain.Common.ValueObjects;

/// <summary>
/// Represents a driver's skill or certification with a proficiency level.
/// </summary>
public record DriverSkill
{
    /// <summary>
    /// The name of the skill or certification (e.g., "CPR", "Wheelchair Securement", "AED").
    /// </summary>
    public string Name { get; init; }

    /// <summary>
    /// The proficiency level on a scale of 1-5, where 1 is beginner and 5 is expert.
    /// </summary>
    public int Level { get; init; }

    /// <summary>
    /// Parameterless constructor for EF Core and JSON serialization.
    /// </summary>
    public DriverSkill()
    {
        Name = string.Empty;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="DriverSkill"/> class.
    /// </summary>
    public DriverSkill(string name, int level)
    {
        Name = name;
        Level = level;
    }
}
