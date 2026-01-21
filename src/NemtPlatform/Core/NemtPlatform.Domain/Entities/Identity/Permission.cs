namespace NemtPlatform.Domain.Entities.Identity;

using NemtPlatform.Domain.Common;

/// <summary>
/// Represents a single granular permission for an action that can be performed in the system.
/// Permissions are system-wide and not tenant-specific.
/// Examples: "trip:create", "billing:view", "fleet:manage"
/// </summary>
public class Permission : Entity
{
    /// <summary>
    /// Gets or sets the human-readable name of the permission.
    /// Example: "Create Trips", "View Billing Reports"
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets an optional description explaining what this permission allows.
    /// Provides additional context about the permission's scope and purpose.
    /// </summary>
    public string? Description { get; set; }
}
