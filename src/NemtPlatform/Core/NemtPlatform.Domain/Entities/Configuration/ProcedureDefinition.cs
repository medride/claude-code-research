namespace NemtPlatform.Domain.Entities.Configuration;

using NemtPlatform.Domain.Common;

/// <summary>
/// Represents a master definition of a stop procedure type.
/// This is a system-wide entity that defines the available procedure types.
/// The Id should match the corresponding StopProcedureType enum value.
/// </summary>
public class ProcedureDefinition : Entity
{
    /// <summary>
    /// Gets or sets the name of the procedure.
    /// This should match the StopProcedureType enum name.
    /// Required field.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets a description of what this procedure entails.
    /// Provides context for users configuring procedure requirements.
    /// Required field.
    /// </summary>
    public string Description { get; set; } = string.Empty;
}
