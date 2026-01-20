namespace NemtPlatform.Domain.Common.ValueObjects;

using NemtPlatform.Domain.Common.Enums;

/// <summary>
/// Template for creating journeys from a standing order.
/// Contains all the information needed to generate actual journey instances.
/// </summary>
/// <param name="FundingSourceId">Foreign key to the FundingSource entity that will pay for generated trips.</param>
/// <param name="CapacityRequirements">Vehicle capacity requirements for the journey.</param>
/// <param name="Legs">Ordered list of leg templates that define the journey structure.</param>
/// <param name="Constraints">Optional trip constraints (driver/vehicle requirements).</param>
/// <param name="CompanionIds">Optional list of foreign keys to TripCompanion entities.</param>
/// <param name="InternalNotes">Optional internal notes for staff reference.</param>
public record JourneyTemplate(
    string FundingSourceId,
    CapacityRequirements CapacityRequirements,
    List<JourneyLegTemplate> Legs,
    TripConstraints? Constraints = null,
    List<string>? CompanionIds = null,
    string? InternalNotes = null);

/// <summary>
/// Template for a single leg within a journey.
/// Defines the stops and transitions for this leg when materialized into a Trip.
/// </summary>
/// <param name="Stops">Ordered list of stop templates for this leg.</param>
/// <param name="TransitionToNext">Optional transition to the next leg.</param>
public record JourneyLegTemplate(
    List<StopTemplate> Stops,
    LegTransition? TransitionToNext = null);

/// <summary>
/// Template for a stop within a journey leg.
/// Contains all information needed to create an actual stop when the journey is materialized.
/// </summary>
/// <param name="Type">The type of stop (Pickup or Dropoff only for journey templates).</param>
/// <param name="AccessPointId">Foreign key to the AccessPoint entity where the stop occurs.</param>
/// <param name="PlaceId">Foreign key to the Place entity associated with this stop.</param>
/// <param name="Duration">Expected duration at this stop.</param>
/// <param name="TimeWindows">List of acceptable time windows for this stop.</param>
/// <param name="ProcedureOverrides">Optional procedure overrides for special handling at this stop.</param>
public record StopTemplate(
    StopType Type,
    string AccessPointId,
    string PlaceId,
    TimeSpan Duration,
    List<TimeWindow> TimeWindows,
    ProcedureOverrides? ProcedureOverrides = null);
