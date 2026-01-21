namespace NemtPlatform.Domain.Entities.Passengers;

using NemtPlatform.Domain.Common;
using NemtPlatform.Domain.Common.Enums;

/// <summary>
/// Represents a person accompanying a passenger on a specific trip.
/// Companions can be registered users (guardians, nurses, case managers) or
/// ad-hoc guests without user accounts. They may have authority to sign for
/// the passenger and perform other trip-related actions.
/// Extends TenantEntity to ensure proper data isolation in multi-tenant scenarios.
/// </summary>
public class TripCompanion : TenantEntity
{
    /// <summary>
    /// Gets or sets the foreign key to the Trip entity.
    /// Required reference to the specific trip this companion is associated with.
    /// </summary>
    public string TripId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the foreign key to the User entity.
    /// Optional reference for registered users in the system (guardians, medical staff).
    /// If null, this is an ad-hoc companion without a user account.
    /// When populated, user information is retrieved from the User entity.
    /// </summary>
    public string? UserId { get; set; }

    /// <summary>
    /// Gets or sets the name of the companion.
    /// Optional field used only for ad-hoc guests who don't have a User record.
    /// If UserId is populated, the name should be retrieved from the User entity instead.
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Gets or sets the role of this companion for the trip.
    /// Determines the level of authority and responsibility during transportation.
    /// Examples: Guardian, Nurse, CaseManager, PersonalCareAssistant.
    /// </summary>
    public TripCompanionRole Role { get; set; }

    /// <summary>
    /// Gets or sets whether this companion is authorized to sign for the passenger.
    /// Important for compliance when the passenger cannot sign for themselves
    /// (e.g., minors, patients with cognitive impairments).
    /// </summary>
    public bool CanSignForPassenger { get; set; }

    /// <summary>
    /// Gets or sets optional notes about this companion for the trip.
    /// May include special instructions, medical credentials, or other relevant information
    /// for drivers and dispatchers.
    /// </summary>
    public string? Notes { get; set; }
}
