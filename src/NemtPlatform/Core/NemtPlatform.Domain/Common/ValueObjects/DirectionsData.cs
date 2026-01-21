namespace NemtPlatform.Domain.Common.ValueObjects;

/// <summary>
/// Represents a distance measurement with both human-readable text and numeric value.
/// </summary>
public record Distance
{
    /// <summary>Human-readable distance string (e.g., "5.2 km", "3.2 mi").</summary>
    public string Text { get; init; } = string.Empty;

    /// <summary>The distance value in meters for calculations.</summary>
    public int ValueInMeters { get; init; }

    /// <summary>Parameterless constructor for EF Core.</summary>
    public Distance() { }

    /// <summary>Creates a new Distance with the specified values.</summary>
    public Distance(string text, int valueInMeters)
    {
        Text = text;
        ValueInMeters = valueInMeters;
    }
}

/// <summary>
/// Represents a time duration with both human-readable text and numeric value.
/// </summary>
public record Duration
{
    /// <summary>Human-readable duration string (e.g., "15 mins", "1 hour 30 mins").</summary>
    public string Text { get; init; } = string.Empty;

    /// <summary>The duration value in seconds for calculations.</summary>
    public int ValueInSeconds { get; init; }

    /// <summary>Parameterless constructor for EF Core.</summary>
    public Duration() { }

    /// <summary>Creates a new Duration with the specified values.</summary>
    public Duration(string text, int valueInSeconds)
    {
        Text = text;
        ValueInSeconds = valueInSeconds;
    }
}

/// <summary>
/// Represents route information obtained from a directions service (e.g., Google Maps API).
/// Contains the encoded polyline path, distance, and duration data.
/// </summary>
public record DirectionsData
{
    /// <summary>The encoded polyline string representing the route path.</summary>
    public string EncodedPolyline { get; init; } = string.Empty;

    /// <summary>The total distance of the route.</summary>
    public Distance? Distance { get; init; }

    /// <summary>The estimated duration to travel the route.</summary>
    public Duration? Duration { get; init; }

    /// <summary>Parameterless constructor for EF Core.</summary>
    public DirectionsData() { }

    /// <summary>Creates a new DirectionsData with the specified values.</summary>
    public DirectionsData(string encodedPolyline, Distance distance, Duration duration)
    {
        EncodedPolyline = encodedPolyline;
        Distance = distance;
        Duration = duration;
    }
}
