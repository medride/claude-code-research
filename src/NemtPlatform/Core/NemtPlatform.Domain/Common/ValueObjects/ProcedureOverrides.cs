namespace NemtPlatform.Domain.Common.ValueObjects;

using NemtPlatform.Domain.Common.Enums;

/// <summary>
/// Defines procedural rule overrides for a specific stop or trip.
/// Allows adding custom procedures or removing default procedures.
/// </summary>
public record ProcedureOverrides
{
    /// <summary>
    /// List of additional procedures to add beyond defaults. Null if no additions.
    /// </summary>
    public List<ProcedureRule>? Add { get; init; }

    /// <summary>
    /// List of procedure types to remove from defaults. Null if no removals.
    /// </summary>
    public List<StopProcedureType>? Remove { get; init; }

    /// <summary>
    /// Parameterless constructor for EF Core and JSON serialization.
    /// </summary>
    public ProcedureOverrides()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ProcedureOverrides"/> class.
    /// </summary>
    public ProcedureOverrides(List<ProcedureRule>? add = null, List<StopProcedureType>? remove = null)
    {
        Add = add;
        Remove = remove;
    }
}
