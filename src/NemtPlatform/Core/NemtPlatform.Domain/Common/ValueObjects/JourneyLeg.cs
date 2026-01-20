namespace NemtPlatform.Domain.Common.ValueObjects;

/// <summary>
/// Represents a single leg (trip) within a multi-leg journey.
/// Each leg has an optional transition that defines how to proceed to the next leg.
/// </summary>
/// <param name="TripId">Foreign key to the Trip entity that represents this leg.</param>
/// <param name="TransitionToNext">Optional transition behavior to the next leg (e.g., wait and return).</param>
public record JourneyLeg(
    string TripId,
    LegTransition? TransitionToNext = null);

/// <summary>
/// Defines the transition behavior between journey legs.
/// Controls how the system handles the period between consecutive legs.
/// </summary>
/// <param name="Type">The type of transition (e.g., "WaitAndReturn" for round trips).</param>
/// <param name="Duration">The duration of the transition period.</param>
public record LegTransition(
    string Type,
    TimeSpan Duration);
