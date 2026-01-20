namespace NemtPlatform.Domain.Entities.Passengers;

using NemtPlatform.Domain.Common;

/// <summary>
/// Represents a person who can act as a guardian for passengers.
/// Guardians can manage trips, view history, and receive notifications on behalf of passengers.
/// This entity extends TenantEntity because guardians belong to a specific tenant organization.
/// Links to the User entity for authentication and authorization.
/// </summary>
public class Guardian : TenantEntity
{
    /// <summary>
    /// Gets or sets the foreign key to the User entity.
    /// Required link to the central authentication record that contains the guardian's
    /// contact information, email, and phone number.
    /// </summary>
    public string UserId { get; set; } = string.Empty;
}
