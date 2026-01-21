namespace NemtPlatform.Domain.Common.ValueObjects;

/// <summary>
/// Contains external identifiers for a trip from integrated broker or partner systems.
/// Enables correlation between internal trips and external trip records.
/// </summary>
public record TripExternalIds
{
    /// <summary>
    /// The trip identifier from the broker's system. Null if not from a broker.
    /// </summary>
    public string? BrokerTripId { get; init; }

    /// <summary>
    /// Parameterless constructor for EF Core and JSON serialization.
    /// </summary>
    public TripExternalIds()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="TripExternalIds"/> class.
    /// </summary>
    public TripExternalIds(string? brokerTripId = null)
    {
        BrokerTripId = brokerTripId;
    }
}
