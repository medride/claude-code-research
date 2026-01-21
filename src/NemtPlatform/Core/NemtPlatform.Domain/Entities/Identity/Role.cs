namespace NemtPlatform.Domain.Entities.Identity;

using NemtPlatform.Domain.Common;

/// <summary>
/// Represents a collection of permissions that can be assigned to users.
/// Roles can be system-wide (TenantId is null) or tenant-specific.
/// System-wide roles example: "SuperAdmin"
/// Tenant-specific roles example: "Dispatcher", "Partner Scheduler"
/// </summary>
public class Role : AuditableEntity
{
    /// <summary>
    /// Gets or sets the identifier of the tenant that owns this role.
    /// If null, this is a system-wide role available across all tenants.
    /// </summary>
    public string? TenantId { get; set; }

    /// <summary>
    /// Gets or sets the name of the role.
    /// Example: "Dispatcher", "Partner Scheduler", "SuperAdmin"
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets an optional description explaining the role's purpose and scope.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Gets or sets the list of permission identifiers associated with this role.
    /// Each string is a foreign key reference to a Permission entity.
    /// </summary>
    public List<string> PermissionIds { get; set; } = new();
}
