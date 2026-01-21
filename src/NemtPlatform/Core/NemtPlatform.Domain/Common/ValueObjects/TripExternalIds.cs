namespace NemtPlatform.Domain.Common.ValueObjects;

/// <summary>
/// Contains external identifiers for a trip from integrated broker or partner systems.
/// Enables correlation between internal trips and external trip records.
/// </summary>
/// <param name="BrokerTripId">The trip identifier from the broker's system. Null if not from a broker.</param>
public record TripExternalIds(string? BrokerTripId = null);
