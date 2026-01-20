namespace NemtPlatform.Domain.Entities.Locations;

using NemtPlatform.Domain.Common;
using NemtPlatform.Domain.Common.Enums;
using NemtPlatform.Domain.Common.ValueObjects;

/// <summary>
/// Specific navigable point at a Place (e.g., "Emergency Room Entrance").
/// Provides precise GPS coordinates and contextual information for drivers
/// to know exactly where to arrive at a facility.
/// </summary>
public class AccessPoint : TenantEntity
{
    /// <summary>
    /// Gets or sets the foreign key to the parent Place.
    /// Required field linking this access point to its facility.
    /// </summary>
    public string PlaceId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the name of this access point.
    /// Required field identifying the specific entrance/exit (e.g., "Emergency Room Entrance", "Main Lobby").
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the precise GPS coordinates for this access point.
    /// Required field providing exact navigation coordinates for drivers.
    /// </summary>
    public GpsLocation Gps { get; set; } = null!;

    /// <summary>
    /// Gets or sets the functional tags/attributes for this access point.
    /// Uses flag enum to support multiple tags (e.g., Entrance | WheelchairAccessible).
    /// Examples: Entrance, Exit, DropOff, PickUp, WheelchairAccessible, StretcherAccessible, etc.
    /// </summary>
    public List<AccessPointTag> Tags { get; set; } = new();

    /// <summary>
    /// Gets or sets the operating hours for this access point.
    /// Optional field specifying when this entrance is available (e.g., "24/7", "Mon-Fri 07:00-18:00").
    /// Null indicates hours are unknown or always available.
    /// </summary>
    public string? OperatingHours { get; set; }

    /// <summary>
    /// Gets or sets special instructions for drivers.
    /// Optional field providing context like parking instructions, security procedures, or navigation tips.
    /// </summary>
    public string? Instructions { get; set; }
}
