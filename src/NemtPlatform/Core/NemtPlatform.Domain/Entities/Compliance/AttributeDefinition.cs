namespace NemtPlatform.Domain.Entities.Compliance;

using NemtPlatform.Domain.Common;

/// <summary>
/// Represents a non-licensed qualification or attribute type.
/// This is a system-wide entity that defines types of qualifications that are not formal licenses,
/// such as background checks, drug tests, safety training, or other compliance requirements.
/// </summary>
public class AttributeDefinition : Entity
{
    /// <summary>
    /// Gets or sets the name of the attribute.
    /// Required field identifying the attribute type (e.g., "Background Check", "Drug Test", "Safety Training").
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the description of the attribute.
    /// Required field providing details about the attribute's purpose and requirements.
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the renewal period in months.
    /// Optional field indicating how often this attribute needs to be renewed.
    /// Null indicates the attribute does not require renewal (e.g., one-time background check).
    /// </summary>
    public int? RenewalPeriodInMonths { get; set; }
}
