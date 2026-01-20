namespace NemtPlatform.Domain.Common.Enums;

/// <summary>
/// Specifies which stop type(s) a procedure rule applies to.
/// </summary>
public enum ProcedureAppliesTo
{
    /// <summary>
    /// The procedure applies only to pickup stops.
    /// </summary>
    Pickup,

    /// <summary>
    /// The procedure applies only to dropoff stops.
    /// </summary>
    Dropoff,

    /// <summary>
    /// The procedure applies to both pickup and dropoff stops.
    /// </summary>
    Any
}
