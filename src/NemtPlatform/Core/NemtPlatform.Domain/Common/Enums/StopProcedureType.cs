namespace NemtPlatform.Domain.Common.Enums;

/// <summary>
/// Enumerates the specific, actionable tasks a driver must perform at a stop.
/// These are often dictated by funding sources or company policy for compliance.
/// </summary>
public enum StopProcedureType
{
    /// <summary>
    /// Requires the passenger's signature.
    /// </summary>
    PassengerSignature,

    /// <summary>
    /// Requires a guardian's signature (for minors or dependent passengers).
    /// </summary>
    GuardianSignature,

    /// <summary>
    /// Requires signature from facility staff.
    /// </summary>
    FacilityStaffSignature,

    /// <summary>
    /// Requires taking a photo at the dropoff location.
    /// </summary>
    PhotoOfDropoff,

    /// <summary>
    /// Requires scanning the patient's ID badge or card.
    /// </summary>
    ScanPatientId,

    /// <summary>
    /// Requires collecting a copayment from the passenger.
    /// </summary>
    CollectCopay,

    /// <summary>
    /// Requires securing the passenger's mobility device (wheelchair, walker, etc.).
    /// </summary>
    SecureMobilityDevice,

    /// <summary>
    /// Requires door-to-door assistance for the passenger.
    /// </summary>
    AssistDoorToDoor,

    /// <summary>
    /// Requires hand-to-hand transfer assistance for the passenger.
    /// </summary>
    AssistHandToHand
}
