namespace NemtPlatform.Domain.Common.ValueObjects;

/// <summary>
/// Represents the passenger capacity requirements for a NEMT vehicle.
/// Defines how many spaces are needed for different mobility requirements.
/// </summary>
/// <param name="WheelchairSpaces">Number of wheelchair-accessible spaces required. Default is 0.</param>
/// <param name="AmbulatorySeats">Number of standard ambulatory passenger seats required. Default is 0.</param>
/// <param name="StretcherCapacity">Number of stretcher/gurney spaces required. Default is 0.</param>
public record CapacityRequirements(
    int WheelchairSpaces = 0,
    int AmbulatorySeats = 0,
    int StretcherCapacity = 0);
