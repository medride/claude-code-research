namespace NemtPlatform.Domain.Entities.Locations;

using NemtPlatform.Domain.Common;
using NemtPlatform.Domain.Common.ValueObjects;

/// <summary>
/// Geographical area for dispatching, service coverage, and analytics.
/// Used to determine service boundaries, route optimization zones, and reporting segments.
/// A region can be defined by either a GeoJSON polygon boundary or a list of ZIP codes.
/// </summary>
public class Region : TenantEntity
{
    /// <summary>
    /// Gets or sets the name of the region.
    /// Required field identifying the service area (e.g., "North County", "Downtown District").
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the GeoJSON polygon defining the region boundary.
    /// Optional field providing precise geographical boundaries for the service area.
    /// If null, the region may be defined by ZipCodes instead.
    /// </summary>
    public GeoJsonPolygon? Boundary { get; set; }

    /// <summary>
    /// Gets or sets the list of ZIP codes included in this region.
    /// Optional field providing an alternative to polygon boundaries.
    /// Useful when service areas align with postal codes rather than custom shapes.
    /// </summary>
    public List<string>? ZipCodes { get; set; }
}
