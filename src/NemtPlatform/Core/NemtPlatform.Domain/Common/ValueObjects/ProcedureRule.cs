namespace NemtPlatform.Domain.Common.ValueObjects;

using NemtPlatform.Domain.Common.Enums;

/// <summary>
/// Defines a specific procedural requirement and when it applies.
/// </summary>
/// <param name="ProcedureId">The type of procedure required at the stop.</param>
/// <param name="AppliesTo">Specifies whether this procedure applies to pickup, dropoff, or both.</param>
public record ProcedureRule(
    StopProcedureType ProcedureId,
    ProcedureAppliesTo AppliesTo);
