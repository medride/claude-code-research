namespace NemtPlatform.Domain.Common.ValueObjects;

/// <summary>
/// Represents a GeoJSON polygon geometry for defining geographical boundaries.
/// Used for region boundaries and geofencing.
/// </summary>
public record GeoJsonPolygon
{
    /// <summary>
    /// The geometry type. Should always be "Polygon" for this record.
    /// </summary>
    public string Type { get; init; }

    /// <summary>
    /// An array of linear rings defining the polygon.
    /// The first ring is the exterior boundary, subsequent rings are holes.
    /// Each ring is an array of positions (longitude, latitude pairs).
    /// The first and last position must be equivalent (closed ring).
    /// </summary>
    public List<List<double[]>> Coordinates { get; init; }

    /// <summary>
    /// Parameterless constructor for EF Core and JSON serialization.
    /// </summary>
    public GeoJsonPolygon()
    {
        Type = string.Empty;
        Coordinates = new();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="GeoJsonPolygon"/> class.
    /// </summary>
    public GeoJsonPolygon(string type, List<List<double[]>> coordinates)
    {
        Type = type;
        Coordinates = coordinates;
    }
}
