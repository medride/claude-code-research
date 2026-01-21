namespace NemtPlatform.Domain.Common.ValueObjects;

/// <summary>
/// Represents the passenger capacity requirements for a NEMT vehicle.
/// Defines how many spaces are needed for different mobility requirements.
/// </summary>
public record CapacityRequirements
{
    /// <summary>
    /// Number of wheelchair-accessible spaces required.
    /// </summary>
    public int WheelchairSpaces { get; init; }

    /// <summary>
    /// Number of standard ambulatory passenger seats required.
    /// </summary>
    public int AmbulatorySeats { get; init; }

    /// <summary>
    /// Number of stretcher/gurney spaces required.
    /// </summary>
    public int StretcherCapacity { get; init; }

    /// <summary>
    /// Parameterless constructor for EF Core and JSON serialization.
    /// </summary>
    public CapacityRequirements()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CapacityRequirements"/> class.
    /// </summary>
    public CapacityRequirements(int wheelchairSpaces = 0, int ambulatorySeats = 0, int stretcherCapacity = 0)
    {
        WheelchairSpaces = wheelchairSpaces;
        AmbulatorySeats = ambulatorySeats;
        StretcherCapacity = stretcherCapacity;
    }
}
