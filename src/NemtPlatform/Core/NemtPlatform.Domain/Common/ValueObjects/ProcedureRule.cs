namespace NemtPlatform.Domain.Common.ValueObjects;

using NemtPlatform.Domain.Common.Enums;

/// <summary>
/// Defines a specific procedural requirement and when it applies.
/// </summary>
public record ProcedureRule
{
    /// <summary>
    /// The type of procedure required at the stop.
    /// </summary>
    public StopProcedureType ProcedureId { get; init; }

    /// <summary>
    /// Specifies whether this procedure applies to pickup, dropoff, or both.
    /// </summary>
    public ProcedureAppliesTo AppliesTo { get; init; }

    /// <summary>
    /// Parameterless constructor for EF Core and JSON serialization.
    /// </summary>
    public ProcedureRule()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ProcedureRule"/> class.
    /// </summary>
    public ProcedureRule(StopProcedureType procedureId, ProcedureAppliesTo appliesTo)
    {
        ProcedureId = procedureId;
        AppliesTo = appliesTo;
    }
}
