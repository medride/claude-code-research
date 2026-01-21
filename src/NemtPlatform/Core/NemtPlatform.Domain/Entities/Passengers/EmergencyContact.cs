namespace NemtPlatform.Domain.Entities.Passengers;

using NemtPlatform.Domain.Common;

/// <summary>
/// Represents an emergency contact for passengers or employees.
/// Stores contact information for people who should be notified in case of emergencies,
/// incidents, or medical situations during transportation.
/// Extends TenantEntity to ensure proper data isolation in multi-tenant scenarios.
/// </summary>
public class EmergencyContact : TenantEntity
{
    /// <summary>
    /// Gets or sets the identifier of the person this emergency contact is for.
    /// Can be a Passenger ID or Employee ID depending on context.
    /// Required reference to link the contact to the appropriate person.
    /// </summary>
    public string PersonId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the full name of the emergency contact.
    /// Required field for identification purposes.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the relationship of the contact to the person.
    /// Examples: "Spouse", "Parent", "Sibling", "Friend", "Caseworker".
    /// Required field for context during emergency communications.
    /// </summary>
    public string Relationship { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the phone number to reach the emergency contact.
    /// Required field as the primary means of emergency communication.
    /// Should be stored in a standardized format for reliable dialing.
    /// </summary>
    public string PhoneNumber { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the priority order for contacting this person.
    /// 1 indicates the primary contact, 2 for secondary, etc.
    /// Used to determine the order in which emergency contacts should be reached.
    /// Lower numbers indicate higher priority.
    /// </summary>
    public int Priority { get; set; } = 1;
}
