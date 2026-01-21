namespace NemtPlatform.Domain.Common.ValueObjects;

/// <summary>
/// Represents a person's name with first and last name components.
/// Provides a FullName property for display purposes.
/// </summary>
public record PersonName
{
    /// <summary>
    /// The person's first (given) name.
    /// </summary>
    public string First { get; init; }

    /// <summary>
    /// The person's last (family) name.
    /// </summary>
    public string Last { get; init; }

    /// <summary>
    /// Parameterless constructor for EF Core and JSON serialization.
    /// </summary>
    public PersonName()
    {
        First = string.Empty;
        Last = string.Empty;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="PersonName"/> class.
    /// </summary>
    public PersonName(string first, string last)
    {
        First = first;
        Last = last;
    }

    /// <summary>
    /// Gets the full name formatted as "First Last".
    /// </summary>
    public string FullName => $"{First} {Last}";
}
