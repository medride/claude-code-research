namespace NemtPlatform.Domain.Common.ValueObjects;

/// <summary>
/// Represents a date range with start and end timestamps.
/// Provides helper methods to check if a date falls within the range or if the range has expired.
/// </summary>
public record DateRange
{
    /// <summary>
    /// The start of the date range (inclusive).
    /// </summary>
    public DateTimeOffset Start { get; init; }

    /// <summary>
    /// The end of the date range (inclusive).
    /// </summary>
    public DateTimeOffset End { get; init; }

    /// <summary>
    /// Parameterless constructor for EF Core and JSON serialization.
    /// </summary>
    public DateRange()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="DateRange"/> class.
    /// </summary>
    public DateRange(DateTimeOffset start, DateTimeOffset end)
    {
        Start = start;
        End = end;
    }

    /// <summary>
    /// Determines whether the specified date falls within this date range (inclusive).
    /// </summary>
    /// <param name="date">The date to check.</param>
    /// <returns>True if the date is within the range; otherwise, false.</returns>
    public bool Contains(DateTimeOffset date) => date >= Start && date <= End;

    /// <summary>
    /// Gets a value indicating whether this date range has expired (end date is in the past).
    /// </summary>
    public bool IsExpired => DateTimeOffset.UtcNow > End;
}
