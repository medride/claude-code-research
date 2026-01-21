namespace NemtPlatform.Domain.Common.ValueObjects;

/// <summary>
/// Defines instructions for driver actions after completing a trip.
/// Typically used for round-trip scenarios where the driver must wait and return the passenger.
/// </summary>
public record PostTripDirective
{
    /// <summary>
    /// The type of post-trip action (e.g., "WaitAndReturn", "Standby").
    /// </summary>
    public string Type { get; init; }

    /// <summary>
    /// How long the driver should wait before the return trip.
    /// </summary>
    public TimeSpan Duration { get; init; }

    /// <summary>
    /// The foreign key to the return trip that will follow this directive.
    /// </summary>
    public string NextTripId { get; init; }

    /// <summary>
    /// Parameterless constructor for EF Core and JSON serialization.
    /// </summary>
    public PostTripDirective()
    {
        Type = string.Empty;
        NextTripId = string.Empty;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="PostTripDirective"/> class.
    /// </summary>
    public PostTripDirective(string type, TimeSpan duration, string nextTripId)
    {
        Type = type;
        Duration = duration;
        NextTripId = nextTripId;
    }
}
