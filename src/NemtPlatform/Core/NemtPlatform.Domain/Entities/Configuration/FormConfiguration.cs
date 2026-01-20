namespace NemtPlatform.Domain.Entities.Configuration;

using NemtPlatform.Domain.Common;
using NemtPlatform.Domain.Common.Enums;

/// <summary>
/// Represents tenant-specific form configuration for dynamic form rendering.
/// Allows customization of forms for different entities and contexts without code changes.
/// </summary>
public class FormConfiguration : TenantEntity
{
    /// <summary>
    /// Gets or sets the name of the entity this configuration applies to.
    /// Examples: "Vehicle", "Passenger", "Trip", "Driver".
    /// Required field.
    /// </summary>
    public string EntityName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the context in which this form configuration applies.
    /// Determines when this specific configuration should be used.
    /// Required field.
    /// </summary>
    public FormContext Context { get; set; }

    /// <summary>
    /// Gets or sets the JSON representation of the form configuration.
    /// Contains field definitions, validation rules, layout information, etc.
    /// Required field.
    /// </summary>
    public string Config { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the version number of this configuration.
    /// Incremented when the configuration is updated to track changes over time.
    /// Required field.
    /// </summary>
    public int Version { get; set; } = 1;

    /// <summary>
    /// Gets or sets whether this configuration is currently active.
    /// Only active configurations are used by the application.
    /// Required field.
    /// </summary>
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// Gets or sets whether this is a system-provided default configuration.
    /// System defaults cannot be deleted but can be overridden by tenant-specific configs.
    /// </summary>
    public bool IsSystemDefault { get; set; } = false;

    /// <summary>
    /// Gets or sets the ID of the configuration this was derived from.
    /// Used to track when a tenant customizes a system default or another configuration.
    /// </summary>
    public string? DerivedFromConfigId { get; set; }

    /// <summary>
    /// Gets or sets tags for categorizing and filtering configurations.
    /// Can be used for organizing related configurations or feature flags.
    /// </summary>
    public List<string>? Tags { get; set; }

    /// <summary>
    /// Gets or sets additional notes or documentation about this configuration.
    /// Useful for describing customizations or the purpose of specific settings.
    /// </summary>
    public string? Notes { get; set; }
}
