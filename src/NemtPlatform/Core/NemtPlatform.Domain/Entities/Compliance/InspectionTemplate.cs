namespace NemtPlatform.Domain.Entities.Compliance;

using NemtPlatform.Domain.Common;
using NemtPlatform.Domain.Common.Enums;
using NemtPlatform.Domain.Common.ValueObjects;

/// <summary>
/// Represents a reusable inspection checklist template.
/// This entity defines the structure and items for vehicle inspections,
/// such as pre-shift or post-shift inspection checklists.
/// </summary>
public class InspectionTemplate : TenantEntity
{
    /// <summary>
    /// Gets or sets the name of the inspection template.
    /// Required field identifying the template (e.g., "Standard Pre-Shift Inspection", "Wheelchair Van Checklist").
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the type of inspection this template is designed for.
    /// Indicates whether this is a pre-shift or post-shift inspection.
    /// </summary>
    public InspectionTemplateType Type { get; set; }

    /// <summary>
    /// Gets or sets the list of checklist items for this inspection.
    /// Required list of items that need to be checked during the inspection,
    /// organized by category with specific prompts for the inspector.
    /// </summary>
    public List<ChecklistItem> ChecklistItems { get; set; } = new();
}
