namespace NemtPlatform.Domain.Common;

/// <summary>
/// Abstract base class for entities in a multi-tenant system.
/// Extends AuditableEntity and adds tenant isolation support.
/// This is the recommended base class for most domain entities.
/// </summary>
public abstract class TenantEntity : AuditableEntity, ITenantEntity
{
    /// <summary>
    /// Gets or sets the identifier of the tenant that owns this entity.
    /// Used for data isolation in multi-tenant scenarios.
    /// </summary>
    public string TenantId { get; set; } = string.Empty;
}
