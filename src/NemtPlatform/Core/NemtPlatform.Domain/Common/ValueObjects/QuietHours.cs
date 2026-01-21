namespace NemtPlatform.Domain.Common.ValueObjects;

/// <summary>
/// Represents a time range during which notifications should not be sent.
/// Used to respect user preferences for quiet periods (e.g., nighttime hours).
/// </summary>
public record QuietHours
{
    /// <summary>The start time of the quiet period.</summary>
    public TimeOnly Start { get; init; }

    /// <summary>The end time of the quiet period.</summary>
    public TimeOnly End { get; init; }

    /// <summary>The IANA timezone identifier (e.g., "America/New_York") for interpreting the time range.</summary>
    public string Timezone { get; init; } = string.Empty;

    /// <summary>Parameterless constructor for EF Core and JSON serialization.</summary>
    public QuietHours() { }

    /// <summary>Creates a new QuietHours with the specified values.</summary>
    public QuietHours(TimeOnly start, TimeOnly end, string timezone)
    {
        Start = start;
        End = end;
        Timezone = timezone;
    }
}
