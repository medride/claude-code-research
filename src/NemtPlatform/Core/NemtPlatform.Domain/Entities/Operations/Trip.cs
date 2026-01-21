namespace NemtPlatform.Domain.Entities.Operations;

using NemtPlatform.Domain.Common;
using NemtPlatform.Domain.Common.Enums;
using NemtPlatform.Domain.Common.ValueObjects;

/// <summary>
/// Represents a core transportation request from origin to destination.
/// A trip contains one pickup stop and one dropoff stop for a single passenger,
/// along with all scheduling, billing, and operational metadata.
/// </summary>
public class Trip : TenantEntity
{
    /// <summary>
    /// Gets or sets the foreign key to the Partner organization that requested this trip.
    /// Null if the trip was created directly by the tenant or passenger.
    /// </summary>
    public string? PartnerId { get; set; }

    /// <summary>
    /// Gets or sets the current lifecycle status of the trip.
    /// Tracks the trip from initial request through approval, scheduling, execution, and completion.
    /// </summary>
    public TripStatus Status { get; set; }

    /// <summary>
    /// Gets or sets the foreign key to the Passenger who will be transported.
    /// Required field. Every trip must have exactly one primary passenger.
    /// </summary>
    public string PassengerId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the foreign key to the FundingSource that pays for this trip.
    /// Required field. Determines billing rules, authorization requirements, and procedures.
    /// </summary>
    public string FundingSourceId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the foreign key to the Journey this trip belongs to.
    /// A journey represents a multi-leg transportation plan (e.g., outbound + return trip).
    /// Null if this is a standalone single trip.
    /// </summary>
    public string? JourneyId { get; set; }

    /// <summary>
    /// Gets or sets the foreign key to the RouteManifest this trip is assigned to.
    /// The manifest represents the driver's scheduled route for a specific day.
    /// Null if the trip is not yet scheduled or assigned.
    /// </summary>
    public string? RouteManifestId { get; set; }

    /// <summary>
    /// Gets or sets the foreign key to the Authorization that approves funding for this trip.
    /// Required by some funding sources (e.g., Medicaid prior authorization).
    /// Null if no authorization is required or not yet obtained.
    /// </summary>
    public string? AuthorizationId { get; set; }

    /// <summary>
    /// Gets or sets external identifiers from integrated broker or partner systems.
    /// Used for tracking and reconciliation with external systems.
    /// Null if no external system integration exists.
    /// </summary>
    public TripExternalIds? ExternalIds { get; set; }

    /// <summary>
    /// Gets or sets the pickup scheduling type.
    /// Determines whether the pickup is scheduled at a fixed time or requested on-demand (will-call).
    /// </summary>
    public PickupType PickupType { get; set; }

    /// <summary>
    /// Gets or sets the list of foreign keys to Companion entities traveling with the passenger.
    /// Companions do not require separate trip records but may affect capacity requirements.
    /// Null or empty if the passenger travels alone.
    /// </summary>
    public List<string>? CompanionIds { get; set; }

    /// <summary>
    /// Gets or sets the foreign key to the Region this trip operates within.
    /// Used for regional service area validation and routing.
    /// Null if regional tracking is not used.
    /// </summary>
    public string? RegionId { get; set; }

    /// <summary>
    /// Gets or sets the foreign key to the ProcedureSet defining default driver tasks.
    /// The procedure set is typically determined by the funding source.
    /// Null if using system default procedures.
    /// </summary>
    public string? ProcedureSetId { get; set; }

    /// <summary>
    /// Gets or sets specific constraints for this trip.
    /// Overrides or supplements default passenger constraints for this trip only.
    /// Null if using only the passenger's default constraints.
    /// </summary>
    public TripConstraints? Constraints { get; set; }

    /// <summary>
    /// Gets or sets the capacity requirements for this trip.
    /// Required field. Defines wheelchair, ambulatory, and stretcher capacity needed.
    /// </summary>
    public CapacityRequirements CapacityRequirements { get; set; } = new();

    /// <summary>
    /// Gets or sets the planned route with directions, distance, and duration.
    /// Typically populated from a mapping service (e.g., Google Maps).
    /// Null if the route has not yet been calculated.
    /// </summary>
    public DirectionsData? PlannedRoute { get; set; }

    /// <summary>
    /// Gets or sets the list of passenger stops for this trip.
    /// Typically contains exactly two stops: one pickup and one dropoff.
    /// </summary>
    public List<PassengerStop> Stops { get; set; } = new();

    /// <summary>
    /// Gets or sets internal operational notes not visible to passengers.
    /// May include dispatch notes, special handling instructions, or coordination details.
    /// </summary>
    public string? InternalNotes { get; set; }

    /// <summary>
    /// Gets or sets the reason this trip was rejected during approval.
    /// Null if the trip was not rejected or is still pending approval.
    /// </summary>
    public string? RejectionReason { get; set; }

    /// <summary>
    /// Gets or sets the reason this trip was canceled.
    /// Null if the trip was not canceled.
    /// </summary>
    public string? CancellationReason { get; set; }

    /// <summary>
    /// Gets or sets instructions for driver actions after completing this trip.
    /// Typically used for round-trip scenarios (e.g., "wait and return").
    /// Null if no post-trip directive is needed.
    /// </summary>
    public PostTripDirective? PostTripDirective { get; set; }
}
