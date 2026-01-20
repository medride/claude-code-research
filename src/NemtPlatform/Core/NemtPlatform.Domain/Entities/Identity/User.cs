namespace NemtPlatform.Domain.Entities.Identity;

using NemtPlatform.Domain.Common;

/// <summary>
/// Central authentication record that represents a user account.
/// This entity extends AuditableEntity (not TenantEntity) because users can span multiple tenants
/// and have different role profiles in each tenant.
/// The Id property matches the authentication provider UID (e.g., Firebase Auth).
/// </summary>
public class User : AuditableEntity
{
    /// <summary>
    /// Gets or sets the user's email address.
    /// Used for authentication and communication.
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the user's phone number.
    /// Optional field for contact and two-factor authentication.
    /// </summary>
    public string? PhoneNumber { get; set; }

    /// <summary>
    /// Gets or sets the user's display name.
    /// Optional friendly name shown in the UI.
    /// </summary>
    public string? DisplayName { get; set; }

    /// <summary>
    /// Gets or sets the URL to the user's profile photo.
    /// Optional avatar image from the authentication provider or uploaded by the user.
    /// </summary>
    public string? PhotoUrl { get; set; }

    /// <summary>
    /// Gets or sets the foreign key to the Employee profile.
    /// Null if the user is not an employee.
    /// </summary>
    public string? EmployeeId { get; set; }

    /// <summary>
    /// Gets or sets the foreign key to the Passenger profile.
    /// Null if the user is not a passenger.
    /// </summary>
    public string? PassengerId { get; set; }

    /// <summary>
    /// Gets or sets the foreign key to the Guardian profile.
    /// Null if the user is not a guardian.
    /// </summary>
    public string? GuardianId { get; set; }

    /// <summary>
    /// Gets or sets the foreign key to the PartnerUser profile.
    /// Null if the user is not associated with a partner organization.
    /// </summary>
    public string? PartnerUserId { get; set; }
}
