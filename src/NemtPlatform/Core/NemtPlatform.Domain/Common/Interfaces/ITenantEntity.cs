namespace NemtPlatform.Domain.Common;

/// <summary>
/// Interface for entities that belong to a specific tenant in a multi-tenant system.
/// </summary>
public interface ITenantEntity : IAuditableEntity
{
    /// <summary>
    /// Gets or sets the identifier of the tenant that owns this entity.
    /// </summary>
    string TenantId { get; set; }
}
