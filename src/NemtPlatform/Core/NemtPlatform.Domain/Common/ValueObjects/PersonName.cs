namespace NemtPlatform.Domain.Common.ValueObjects;

/// <summary>
/// Represents a person's name with first and last name components.
/// Provides a FullName property for display purposes.
/// </summary>
/// <param name="First">The person's first (given) name.</param>
/// <param name="Last">The person's last (family) name.</param>
public record PersonName(string First, string Last)
{
    /// <summary>
    /// Gets the full name formatted as "First Last".
    /// </summary>
    public string FullName => $"{First} {Last}";
}
