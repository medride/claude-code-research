namespace NemtPlatform.Domain.Common.ValueObjects;

/// <summary>
/// Represents a geographic coordinate using GPS latitude and longitude.
/// </summary>
public record GpsLocation
{
    /// <summary>
    /// The latitude coordinate in decimal degrees. Valid range: -90 to 90.
    /// </summary>
    public double Latitude { get; init; }

    /// <summary>
    /// The longitude coordinate in decimal degrees. Valid range: -180 to 180.
    /// </summary>
    public double Longitude { get; init; }

    /// <summary>
    /// Parameterless constructor for EF Core and JSON serialization.
    /// </summary>
    public GpsLocation()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="GpsLocation"/> class.
    /// </summary>
    public GpsLocation(double latitude, double longitude)
    {
        Latitude = latitude;
        Longitude = longitude;
    }
}
