namespace NemtPlatform.Domain.Common.Enums;

/// <summary>
/// Defines the role of a person accompanying a passenger on a specific trip.
/// Determines the level of authority and responsibility during the trip.
/// </summary>
public enum TripCompanionRole
{
    /// <summary>
    /// A legal guardian or parent accompanying the passenger.
    /// </summary>
    Guardian,

    /// <summary>
    /// A medical professional providing care during transport.
    /// </summary>
    Nurse,

    /// <summary>
    /// A case manager or social worker overseeing the passenger's care.
    /// </summary>
    CaseManager,

    /// <summary>
    /// A personal care assistant or aide.
    /// </summary>
    PersonalCareAssistant
}
