namespace NemtPlatform.Domain.Entities.Locations;

using NemtPlatform.Domain.Common;
using NemtPlatform.Domain.Common.Enums;
using NemtPlatform.Domain.Common.ValueObjects;

/// <summary>
/// Master record for a facility or location (e.g., "Mercy General Hospital").
/// Represents a physical place that can be a pickup or dropoff destination.
/// A Place may have multiple AccessPoints for different entry/exit scenarios.
/// </summary>
public class Place : TenantEntity
{
    /// <summary>
    /// Gets or sets the name of the place.
    /// Required field identifying the facility or location (e.g., "Mercy General Hospital", "John Smith Residence").
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the type/category of this place.
    /// Determines the general classification (Hospital, Clinic, School, Residence, etc.).
    /// </summary>
    public PlaceType Type { get; set; }

    /// <summary>
    /// Gets or sets the physical address of the place.
    /// Required field for navigation and documentation purposes.
    /// </summary>
    public Address Address { get; set; } = null!;

    /// <summary>
    /// Gets or sets the central GPS coordinate for this place.
    /// Optional field representing a general location point for the facility.
    /// For precise navigation, use specific AccessPoint GPS coordinates instead.
    /// </summary>
    public GpsLocation? CenterGps { get; set; }
}
