namespace NemtPlatform.Domain.Common.ValueObjects;

/// <summary>
/// Represents route information obtained from a directions service (e.g., Google Maps API).
/// Contains the encoded polyline path, distance, and duration data.
/// </summary>
/// <param name="EncodedPolyline">The encoded polyline string representing the route path.</param>
/// <param name="Distance">The total distance of the route.</param>
/// <param name="Duration">The estimated duration to travel the route.</param>
public record DirectionsData(
    string EncodedPolyline,
    Distance Distance,
    Duration Duration);

/// <summary>
/// Represents a distance measurement with both human-readable text and numeric value.
/// </summary>
/// <param name="Text">Human-readable distance string (e.g., "5.2 km", "3.2 mi").</param>
/// <param name="ValueInMeters">The distance value in meters for calculations.</param>
public record Distance(string Text, int ValueInMeters);

/// <summary>
/// Represents a time duration with both human-readable text and numeric value.
/// </summary>
/// <param name="Text">Human-readable duration string (e.g., "15 mins", "1 hour 30 mins").</param>
/// <param name="ValueInSeconds">The duration value in seconds for calculations.</param>
public record Duration(string Text, int ValueInSeconds);
