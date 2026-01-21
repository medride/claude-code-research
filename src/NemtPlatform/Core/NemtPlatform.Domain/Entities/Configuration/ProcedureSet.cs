namespace NemtPlatform.Domain.Entities.Configuration;

using NemtPlatform.Domain.Common;
using NemtPlatform.Domain.Common.ValueObjects;

/// <summary>
/// Represents a named collection of procedure rules that can be applied to trips or funding sources.
/// This allows tenants to define reusable sets of procedural requirements.
/// </summary>
public class ProcedureSet : TenantEntity
{
    /// <summary>
    /// Gets or sets the name of the procedure set.
    /// Used for identification when assigning to trips or funding sources.
    /// Required field.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the list of procedure rules that comprise this set.
    /// Each rule specifies a procedure type and when it applies (pickup, dropoff, or both).
    /// Required field.
    /// </summary>
    public List<ProcedureRule> Procedures { get; set; } = new();
}
