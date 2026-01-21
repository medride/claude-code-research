namespace NemtPlatform.Domain.Common.Enums;

/// <summary>
/// Defines the cognitive status of a patient.
/// Critical for drivers to understand patient's needs and potential wander risk.
/// </summary>
public enum CognitiveStatus
{
    /// <summary>
    /// Patient is fully aware of their surroundings and oriented to person, place, and time.
    /// </summary>
    AlertAndOriented,

    /// <summary>
    /// Patient has minor cognitive difficulties but can generally function independently.
    /// </summary>
    MildImpairment,

    /// <summary>
    /// Patient has significant cognitive difficulties and may require assistance.
    /// </summary>
    ModerateImpairment,

    /// <summary>
    /// Patient has severe cognitive impairment and requires substantial supervision.
    /// </summary>
    SevereImpairment
}
