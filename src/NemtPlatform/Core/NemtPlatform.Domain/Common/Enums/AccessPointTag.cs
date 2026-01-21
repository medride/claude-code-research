namespace NemtPlatform.Domain.Common.Enums;

/// <summary>
/// Defines the functional attributes of an AccessPoint.
/// An AccessPoint can have multiple tags using bitwise combinations.
/// </summary>
[Flags]
public enum AccessPointTag
{
    /// <summary>
    /// No tags assigned.
    /// </summary>
    None = 0,

    /// <summary>
    /// Designated as an entrance point.
    /// </summary>
    Entrance = 1,

    /// <summary>
    /// Designated as an exit point.
    /// </summary>
    Exit = 2,

    /// <summary>
    /// Designated for passenger dropoff.
    /// </summary>
    DropOff = 4,

    /// <summary>
    /// Designated for passenger pickup.
    /// </summary>
    PickUp = 8,

    /// <summary>
    /// Accessible for wheelchair users.
    /// </summary>
    WheelchairAccessible = 16,

    /// <summary>
    /// Accessible for stretcher transport.
    /// </summary>
    StretcherAccessible = 32,

    /// <summary>
    /// Designated ambulance bay area.
    /// </summary>
    AmbulanceBay = 64,

    /// <summary>
    /// Designated parent dropoff zone.
    /// </summary>
    ParentDropOff = 128,

    /// <summary>
    /// Designated bus zone area.
    /// </summary>
    BusZone = 256
}
