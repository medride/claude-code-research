namespace NemtPlatform.Domain.Common.Enums;

/// <summary>
/// Represents the operational role of a crew member during a shift.
/// </summary>
public enum CrewRole
{
    /// <summary>
    /// The person responsible for operating the vehicle.
    /// </summary>
    Driver,

    /// <summary>
    /// Emergency Medical Technician providing basic emergency medical care.
    /// </summary>
    Emt,

    /// <summary>
    /// Advanced medical professional providing emergency medical services.
    /// </summary>
    Paramedic,

    /// <summary>
    /// An assistant who helps with passenger care and boarding.
    /// </summary>
    Attendant,

    /// <summary>
    /// Any other role not specifically categorized.
    /// </summary>
    Other
}
