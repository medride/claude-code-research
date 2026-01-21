namespace NemtPlatform.Domain.Common.ValueObjects;

/// <summary>
/// Defines the transition behavior between journey legs.
/// Controls how the system handles the period between consecutive legs.
/// </summary>
public record LegTransition
{
    /// <summary>The type of transition (e.g., "WaitAndReturn" for round trips).</summary>
    public string Type { get; init; } = string.Empty;

    /// <summary>The duration of the transition period.</summary>
    public TimeSpan Duration { get; init; }

    /// <summary>Parameterless constructor for EF Core and JSON serialization.</summary>
    public LegTransition() { }

    /// <summary>Creates a new LegTransition with the specified values.</summary>
    public LegTransition(string type, TimeSpan duration)
    {
        Type = type;
        Duration = duration;
    }
}

/// <summary>
/// Represents a single leg (trip) within a multi-leg journey.
/// Each leg has an optional transition that defines how to proceed to the next leg.
/// </summary>
public record JourneyLeg
{
    /// <summary>Foreign key to the Trip entity that represents this leg.</summary>
    public string TripId { get; init; } = string.Empty;

    /// <summary>Optional transition behavior to the next leg (e.g., wait and return).</summary>
    public LegTransition? TransitionToNext { get; init; }

    /// <summary>Parameterless constructor for EF Core and JSON serialization.</summary>
    public JourneyLeg() { }

    /// <summary>Creates a new JourneyLeg with the specified values.</summary>
    public JourneyLeg(string tripId, LegTransition? transitionToNext = null)
    {
        TripId = tripId;
        TransitionToNext = transitionToNext;
    }
}
