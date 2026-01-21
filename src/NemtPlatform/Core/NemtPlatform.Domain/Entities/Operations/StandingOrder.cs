namespace NemtPlatform.Domain.Entities.Operations;

using NemtPlatform.Domain.Common;
using NemtPlatform.Domain.Common.Enums;
using NemtPlatform.Domain.Common.ValueObjects;

/// <summary>
/// Represents a recurring transportation need template (standing order).
/// Standing orders automatically generate journey instances based on a recurrence pattern.
/// Common use cases include weekly dialysis appointments, recurring medical treatments, or daily school transport.
/// </summary>
public class StandingOrder : TenantEntity
{
    /// <summary>
    /// Gets or sets the foreign key to the Partner if this standing order was created by a partner organization.
    /// Null if created directly by the tenant.
    /// </summary>
    public string? PartnerId { get; set; }

    /// <summary>
    /// Gets or sets the human-readable name for this standing order.
    /// Required field for easier identification (e.g., "John's Weekly Dialysis").
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the foreign key to the Passenger who will receive the recurring transportation.
    /// Required field as every standing order must be associated with a passenger.
    /// </summary>
    public string PassengerId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the current status of this standing order.
    /// Controls whether new journeys are being generated.
    /// Required field - defaults to Active when created.
    /// </summary>
    public StandingOrderStatus Status { get; set; }

    /// <summary>
    /// Gets or sets the recurrence pattern in iCalendar RRULE format.
    /// Required field that defines when journeys should be generated.
    /// Example: "FREQ=WEEKLY;BYDAY=MO,WE,FR" for Monday, Wednesday, Friday.
    /// </summary>
    public string RecurrenceRule { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the list of dates that should be excluded from journey generation.
    /// Optional field for handling holidays, vacations, or one-time cancellations.
    /// Dates are stored in YYYY-MM-DD format.
    /// </summary>
    public List<string>? ExclusionDates { get; set; }

    /// <summary>
    /// Gets or sets the date range during which this standing order is effective.
    /// Required field that defines the start and end dates for journey generation.
    /// No journeys will be generated outside this range.
    /// </summary>
    public DateRange EffectiveDateRange { get; set; } = null!;

    /// <summary>
    /// Gets or sets the journey template used to create journey instances.
    /// Required field containing all the information needed to generate actual journeys.
    /// Includes funding source, capacity requirements, leg templates, and constraints.
    /// </summary>
    public JourneyTemplate JourneyTemplate { get; set; } = null!;

    /// <summary>
    /// Gets or sets the date up to which journeys have been generated.
    /// Optional field used by the journey generation process to track progress.
    /// Null if no journeys have been generated yet.
    /// </summary>
    public DateTimeOffset? LastGeneratedUpToDate { get; set; }
}
