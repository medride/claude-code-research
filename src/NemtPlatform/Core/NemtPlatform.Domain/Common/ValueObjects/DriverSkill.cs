namespace NemtPlatform.Domain.Common.ValueObjects;

/// <summary>
/// Represents a driver's skill or certification with a proficiency level.
/// </summary>
/// <param name="Name">The name of the skill or certification (e.g., "CPR", "Wheelchair Securement", "AED").</param>
/// <param name="Level">The proficiency level on a scale of 1-5, where 1 is beginner and 5 is expert.</param>
public record DriverSkill(string Name, int Level);
