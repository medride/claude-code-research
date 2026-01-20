namespace NemtPlatform.Domain.Common.Enums;

/// <summary>
/// Defines the possible categories for a Place.
/// </summary>
public enum PlaceType
{
    /// <summary>
    /// A hospital facility.
    /// </summary>
    Hospital,

    /// <summary>
    /// A medical clinic.
    /// </summary>
    Clinic,

    /// <summary>
    /// An educational institution.
    /// </summary>
    School,

    /// <summary>
    /// A residential location.
    /// </summary>
    Residence,

    /// <summary>
    /// A commercial business location.
    /// </summary>
    Business,

    /// <summary>
    /// For locations without a building, like a street corner bus stop.
    /// </summary>
    Intersection,

    /// <summary>
    /// An airport facility.
    /// </summary>
    Airport
}
