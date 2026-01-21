namespace NemtPlatform.Domain.Common.ValueObjects;

/// <summary>
/// Represents a single item in an inspection checklist.
/// </summary>
public record ChecklistItem
{
    /// <summary>
    /// The category or system this checklist item belongs to (e.g., "Brakes", "Lights", "Safety Equipment").
    /// </summary>
    public string Category { get; init; }

    /// <summary>
    /// The specific instruction or question to check (e.g., "Check brake pedal resistance", "Verify all lights are operational").
    /// </summary>
    public string Prompt { get; init; }

    /// <summary>
    /// Parameterless constructor for EF Core and JSON serialization.
    /// </summary>
    public ChecklistItem()
    {
        Category = string.Empty;
        Prompt = string.Empty;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ChecklistItem"/> class.
    /// </summary>
    public ChecklistItem(string category, string prompt)
    {
        Category = category;
        Prompt = prompt;
    }
}
