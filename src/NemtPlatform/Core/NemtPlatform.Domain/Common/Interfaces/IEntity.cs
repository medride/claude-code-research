namespace NemtPlatform.Domain.Common;

/// <summary>
/// Base interface for all entities in the domain.
/// </summary>
public interface IEntity
{
    /// <summary>
    /// Gets or sets the unique identifier for the entity.
    /// </summary>
    string Id { get; set; }
}
