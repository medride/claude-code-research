namespace NemtPlatform.Domain.Common.ValueObjects;

/// <summary>
/// Represents a time range during which notifications should not be sent.
/// Used to respect user preferences for quiet periods (e.g., nighttime hours).
/// </summary>
/// <param name="Start">The start time of the quiet period.</param>
/// <param name="End">The end time of the quiet period.</param>
/// <param name="Timezone">The IANA timezone identifier (e.g., "America/New_York") for interpreting the time range.</param>
public record QuietHours(TimeOnly Start, TimeOnly End, string Timezone);
