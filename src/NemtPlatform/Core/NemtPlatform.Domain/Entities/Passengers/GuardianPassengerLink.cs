namespace NemtPlatform.Domain.Entities.Passengers;

using NemtPlatform.Domain.Common;
using NemtPlatform.Domain.Common.Enums;
using NemtPlatform.Domain.Common.ValueObjects;

/// <summary>
/// Links a Guardian to a Passenger with specific permissions and relationship details.
/// This is a many-to-many relationship entity that defines what actions a guardian
/// can perform for a specific passenger and how they should be notified.
/// Extends TenantEntity to ensure proper data isolation in multi-tenant scenarios.
/// </summary>
public class GuardianPassengerLink : TenantEntity
{
    /// <summary>
    /// Gets or sets the foreign key to the Guardian entity.
    /// Required reference to the person acting as guardian.
    /// </summary>
    public string GuardianId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the foreign key to the Passenger entity.
    /// Required reference to the passenger being cared for.
    /// </summary>
    public string PassengerId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the type of relationship between the guardian and passenger.
    /// Determines legal authority and operational procedures.
    /// </summary>
    public GuardianRelationship Relationship { get; set; }

    /// <summary>
    /// Gets or sets the permissions granted to this guardian for this passenger.
    /// Controls what actions the guardian can perform (scheduling, billing, viewing history)
    /// and whether they receive notifications for trip events.
    /// </summary>
    public GuardianPermissions Permissions { get; set; } = new();

    /// <summary>
    /// Gets or sets the notification preferences for this guardian relationship.
    /// Optional setting that defines how and when the guardian wants to be notified
    /// about the passenger's trips, including preferred channels and quiet hours.
    /// </summary>
    public NotificationPreferences? NotificationPreferences { get; set; }
}
