namespace NemtPlatform.Domain.Common.ValueObjects;

using NemtPlatform.Domain.Common.Enums;

/// <summary>
/// Template for creating journeys from a standing order.
/// Contains all the information needed to generate actual journey instances.
/// </summary>
public record JourneyTemplate
{
    /// <summary>
    /// Foreign key to the FundingSource entity that will pay for generated trips.
    /// </summary>
    public string FundingSourceId { get; init; }

    /// <summary>
    /// Vehicle capacity requirements for the journey.
    /// </summary>
    public CapacityRequirements CapacityRequirements { get; init; }

    /// <summary>
    /// Ordered list of leg templates that define the journey structure.
    /// </summary>
    public List<JourneyLegTemplate> Legs { get; init; }

    /// <summary>
    /// Optional trip constraints (driver/vehicle requirements).
    /// </summary>
    public TripConstraints? Constraints { get; init; }

    /// <summary>
    /// Optional list of foreign keys to TripCompanion entities.
    /// </summary>
    public List<string>? CompanionIds { get; init; }

    /// <summary>
    /// Optional internal notes for staff reference.
    /// </summary>
    public string? InternalNotes { get; init; }

    /// <summary>
    /// Parameterless constructor for EF Core and JSON serialization.
    /// </summary>
    public JourneyTemplate()
    {
        FundingSourceId = string.Empty;
        CapacityRequirements = new();
        Legs = new();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="JourneyTemplate"/> class.
    /// </summary>
    public JourneyTemplate(
        string fundingSourceId,
        CapacityRequirements capacityRequirements,
        List<JourneyLegTemplate> legs,
        TripConstraints? constraints = null,
        List<string>? companionIds = null,
        string? internalNotes = null)
    {
        FundingSourceId = fundingSourceId;
        CapacityRequirements = capacityRequirements;
        Legs = legs;
        Constraints = constraints;
        CompanionIds = companionIds;
        InternalNotes = internalNotes;
    }
}

/// <summary>
/// Template for a single leg within a journey.
/// Defines the stops and transitions for this leg when materialized into a Trip.
/// </summary>
public record JourneyLegTemplate
{
    /// <summary>
    /// Ordered list of stop templates for this leg.
    /// </summary>
    public List<StopTemplate> Stops { get; init; }

    /// <summary>
    /// Optional transition to the next leg.
    /// </summary>
    public LegTransition? TransitionToNext { get; init; }

    /// <summary>
    /// Parameterless constructor for EF Core and JSON serialization.
    /// </summary>
    public JourneyLegTemplate()
    {
        Stops = new();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="JourneyLegTemplate"/> class.
    /// </summary>
    public JourneyLegTemplate(List<StopTemplate> stops, LegTransition? transitionToNext = null)
    {
        Stops = stops;
        TransitionToNext = transitionToNext;
    }
}

/// <summary>
/// Template for a stop within a journey leg.
/// Contains all information needed to create an actual stop when the journey is materialized.
/// </summary>
public record StopTemplate
{
    /// <summary>
    /// The type of stop (Pickup or Dropoff only for journey templates).
    /// </summary>
    public StopType Type { get; init; }

    /// <summary>
    /// Foreign key to the AccessPoint entity where the stop occurs.
    /// </summary>
    public string AccessPointId { get; init; }

    /// <summary>
    /// Foreign key to the Place entity associated with this stop.
    /// </summary>
    public string PlaceId { get; init; }

    /// <summary>
    /// Expected duration at this stop.
    /// </summary>
    public TimeSpan Duration { get; init; }

    /// <summary>
    /// List of acceptable time windows for this stop.
    /// </summary>
    public List<TimeWindow> TimeWindows { get; init; }

    /// <summary>
    /// Optional procedure overrides for special handling at this stop.
    /// </summary>
    public ProcedureOverrides? ProcedureOverrides { get; init; }

    /// <summary>
    /// Parameterless constructor for EF Core and JSON serialization.
    /// </summary>
    public StopTemplate()
    {
        AccessPointId = string.Empty;
        PlaceId = string.Empty;
        TimeWindows = new();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="StopTemplate"/> class.
    /// </summary>
    public StopTemplate(
        StopType type,
        string accessPointId,
        string placeId,
        TimeSpan duration,
        List<TimeWindow> timeWindows,
        ProcedureOverrides? procedureOverrides = null)
    {
        Type = type;
        AccessPointId = accessPointId;
        PlaceId = placeId;
        Duration = duration;
        TimeWindows = timeWindows;
        ProcedureOverrides = procedureOverrides;
    }
}
