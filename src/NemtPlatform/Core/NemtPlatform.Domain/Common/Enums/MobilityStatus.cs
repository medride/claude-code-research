namespace NemtPlatform.Domain.Common.Enums;

/// <summary>
/// Defines the mobility status of a patient for medical transport (NEMT).
/// Determines the type of vehicle and equipment required.
/// </summary>
public enum MobilityStatus
{
    /// <summary>
    /// Patient can walk independently and does not require mobility assistance.
    /// </summary>
    Ambulatory,

    /// <summary>
    /// Patient requires a wheelchair for mobility.
    /// Requires wheelchair-accessible vehicle with lift or ramp.
    /// </summary>
    Wheelchair,

    /// <summary>
    /// Patient must remain in a lying position during transport.
    /// Requires specialized stretcher-equipped vehicle.
    /// </summary>
    Stretcher
}
