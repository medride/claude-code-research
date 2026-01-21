namespace NemtPlatform.Domain.Common.ValueObjects;

/// <summary>
/// Defines instructions for driver actions after completing a trip.
/// Typically used for round-trip scenarios where the driver must wait and return the passenger.
/// </summary>
/// <param name="Type">The type of post-trip action (e.g., "WaitAndReturn", "Standby").</param>
/// <param name="Duration">How long the driver should wait before the return trip.</param>
/// <param name="NextTripId">The foreign key to the return trip that will follow this directive.</param>
public record PostTripDirective(
    string Type,
    TimeSpan Duration,
    string NextTripId);
