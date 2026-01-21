namespace NemtPlatform.Domain.Common;

/// <summary>
/// Abstract base class for entities that support audit tracking.
/// Extends the base Entity class and implements audit properties.
/// </summary>
public abstract class AuditableEntity : Entity, IAuditableEntity
{
    /// <summary>
    /// Gets or sets the date and time when the entity was created.
    /// </summary>
    public DateTimeOffset CreatedAt { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the entity was last updated.
    /// </summary>
    public DateTimeOffset? UpdatedAt { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the user who created the entity.
    /// </summary>
    public string CreatedBy { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the identifier of the user who last updated the entity.
    /// </summary>
    public string? UpdatedBy { get; set; }
}
