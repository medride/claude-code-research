namespace NemtPlatform.Domain.Common.Enums;

/// <summary>
/// Represents the medical service level a vehicle can provide.
/// </summary>
public enum MedicalServiceLevel
{
    /// <summary>
    /// Basic Life Support - standard emergency medical services.
    /// </summary>
    Bls,

    /// <summary>
    /// Advanced Life Support - enhanced emergency medical services with advanced interventions.
    /// </summary>
    Als,

    /// <summary>
    /// Critical Care Transport - highest level of medical transport for critically ill patients.
    /// </summary>
    Cct
}
