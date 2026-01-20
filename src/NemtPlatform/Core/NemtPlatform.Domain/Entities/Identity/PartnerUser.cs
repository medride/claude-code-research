namespace NemtPlatform.Domain.Entities.Identity;

using NemtPlatform.Domain.Common;

/// <summary>
/// Represents a user profile for an employee of a partner organization.
/// Partner users have access to the system to book trips on behalf of their patients.
/// Examples: Hospital schedulers, clinic coordinators, social workers.
/// </summary>
public class PartnerUser : TenantEntity
{
    /// <summary>
    /// Gets or sets the identifier of the central user record.
    /// Foreign key reference to the User entity containing authentication details.
    /// </summary>
    public string UserId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the identifier of the partner organization this user belongs to.
    /// Foreign key reference to the Partner entity.
    /// </summary>
    public string PartnerId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the list of role identifiers assigned to this partner user.
    /// Each string is a foreign key reference to a Role entity.
    /// Determines what actions this partner user can perform in the system.
    /// </summary>
    public List<string> RoleIds { get; set; } = new();

    /// <summary>
    /// Gets or sets the job title of the partner user within their organization.
    /// Example: "Scheduling Coordinator", "Patient Services Manager", "Medical Assistant"
    /// </summary>
    public string? JobTitle { get; set; }
}
