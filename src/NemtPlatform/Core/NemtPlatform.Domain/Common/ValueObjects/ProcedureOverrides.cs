namespace NemtPlatform.Domain.Common.ValueObjects;

using NemtPlatform.Domain.Common.Enums;

/// <summary>
/// Defines procedural rule overrides for a specific stop or trip.
/// Allows adding custom procedures or removing default procedures.
/// </summary>
/// <param name="Add">List of additional procedures to add beyond defaults. Null if no additions.</param>
/// <param name="Remove">List of procedure types to remove from defaults. Null if no removals.</param>
public record ProcedureOverrides(
    List<ProcedureRule>? Add = null,
    List<StopProcedureType>? Remove = null);
