namespace NemtPlatform.Domain.Common;

/// <summary>
/// Abstract base class for all entities in the domain.
/// Implements the base entity interface with a string-based identifier.
/// </summary>
public abstract class Entity : IEntity
{
    /// <summary>
    /// Gets or sets the unique identifier for the entity.
    /// Uses GUID strings for global uniqueness.
    /// </summary>
    public string Id { get; set; } = Guid.NewGuid().ToString();
}
