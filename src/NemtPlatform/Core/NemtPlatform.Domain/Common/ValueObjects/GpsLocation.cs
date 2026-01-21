namespace NemtPlatform.Domain.Common.ValueObjects;

/// <summary>
/// Represents a geographic coordinate using GPS latitude and longitude.
/// </summary>
/// <param name="Latitude">The latitude coordinate in decimal degrees. Valid range: -90 to 90.</param>
/// <param name="Longitude">The longitude coordinate in decimal degrees. Valid range: -180 to 180.</param>
public record GpsLocation(double Latitude, double Longitude);
