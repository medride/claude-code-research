namespace NemtPlatform.Domain.Common;

/// <summary>
/// Interface for entities that support audit tracking.
/// Tracks who created and modified the entity and when.
/// </summary>
public interface IAuditableEntity : IEntity
{
    /// <summary>
    /// Gets or sets the date and time when the entity was created.
    /// </summary>
    DateTimeOffset CreatedAt { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the entity was last updated.
    /// </summary>
    DateTimeOffset? UpdatedAt { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the user who created the entity.
    /// </summary>
    string CreatedBy { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the user who last updated the entity.
    /// </summary>
    string? UpdatedBy { get; set; }
}
