namespace NemtPlatform.Domain.Entities.Configuration;

using NemtPlatform.Domain.Common;
using NemtPlatform.Domain.Common.ValueObjects;

/// <summary>
/// Represents user-specific view preferences for data tables and lists.
/// Allows users to customize column visibility, order, sizes, and sorting.
/// </summary>
public class ViewConfiguration : TenantEntity
{
    /// <summary>
    /// Gets or sets the identifier of the user who owns this view configuration.
    /// Each user has their own set of view preferences.
    /// Required field.
    /// </summary>
    public string UserId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the identifier of the specific view this configuration applies to.
    /// Examples: "trips_list", "vehicles_table", "drivers_grid".
    /// Required field.
    /// </summary>
    public string ViewId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the name of the entity being viewed.
    /// Examples: "Trip", "Vehicle", "Driver".
    /// Optional - some views may be composite or not entity-specific.
    /// </summary>
    public string? EntityName { get; set; }

    /// <summary>
    /// Gets or sets the user's preferences for this view.
    /// Includes column order, visibility, sizing, and sorting settings.
    /// Required field.
    /// </summary>
    public ViewPreferences Preferences { get; set; } = new();
}
