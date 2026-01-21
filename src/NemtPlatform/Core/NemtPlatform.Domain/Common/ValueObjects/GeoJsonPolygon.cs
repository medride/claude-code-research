namespace NemtPlatform.Domain.Common.ValueObjects;

/// <summary>
/// Represents a GeoJSON polygon geometry for defining geographical boundaries.
/// Used for region boundaries and geofencing.
/// </summary>
/// <param name="Type">The geometry type. Should always be "Polygon" for this record.</param>
/// <param name="Coordinates">An array of linear rings defining the polygon.
/// The first ring is the exterior boundary, subsequent rings are holes.
/// Each ring is an array of positions (longitude, latitude pairs).
/// The first and last position must be equivalent (closed ring).</param>
public record GeoJsonPolygon(
    string Type,
    List<List<double[]>> Coordinates);
