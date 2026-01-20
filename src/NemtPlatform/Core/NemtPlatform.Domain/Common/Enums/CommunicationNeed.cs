namespace NemtPlatform.Domain.Common.Enums;

/// <summary>
/// Defines special communication needs for passengers.
/// Informs crew how to best interact with the passenger.
/// </summary>
public enum CommunicationNeed
{
    /// <summary>
    /// Passenger is non-verbal and uses alternative communication methods.
    /// </summary>
    NonVerbal,

    /// <summary>
    /// Passenger has hearing impairment and may require visual communication.
    /// </summary>
    HearingImpaired,

    /// <summary>
    /// Passenger requires a Spanish language translator.
    /// </summary>
    RequiresTranslatorSpanish
}
