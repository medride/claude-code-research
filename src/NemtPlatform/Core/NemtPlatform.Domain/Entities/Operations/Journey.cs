namespace NemtPlatform.Domain.Entities.Operations;

using NemtPlatform.Domain.Common;
using NemtPlatform.Domain.Common.ValueObjects;

/// <summary>
/// Represents a sequence of related trips that form a complete transportation journey.
/// A journey groups multiple trip legs together (e.g., outbound and return trips in a round trip).
/// Journeys can be created manually or generated from a standing order template.
/// </summary>
public class Journey : TenantEntity
{
    /// <summary>
    /// Gets or sets the foreign key to the Partner if this journey was created by a partner organization.
    /// Null if created directly by the tenant.
    /// </summary>
    public string? PartnerId { get; set; }

    /// <summary>
    /// Gets or sets the foreign key to the Passenger who is traveling on this journey.
    /// Required field as every journey must be associated with a passenger.
    /// </summary>
    public string PassengerId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the ordered list of legs (trips) that comprise this journey.
    /// Each leg contains a trip ID and optional transition to the next leg.
    /// Required field - a journey must have at least one leg.
    /// </summary>
    public List<JourneyLeg> Legs { get; set; } = null!;

    /// <summary>
    /// Gets or sets the human-readable name for this journey.
    /// Optional field for easier identification (e.g., "Weekly Dialysis - Monday").
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Gets or sets the date and time when this journey was booked.
    /// Required field for tracking and auditing purposes.
    /// </summary>
    public DateTimeOffset BookingDate { get; set; }

    /// <summary>
    /// Gets or sets the foreign key to the StandingOrder that generated this journey.
    /// Null if this journey was created manually (not from a recurring template).
    /// Used to track which standing order generated this journey instance.
    /// </summary>
    public string? SourceStandingOrderId { get; set; }
}
