namespace NemtPlatform.Domain.Common.ValueObjects;

/// <summary>
/// Represents the recipient who received the passenger at the end of a trip segment.
/// Used for accountability and compliance tracking in passenger handoffs.
/// </summary>
/// <param name="Type">The type of recipient who received the passenger.</param>
/// <param name="Name">The optional name of the recipient. May be null for self-handoffs.</param>
public record HandOffRecipient(
    HandOffRecipientType Type,
    string? Name = null);

/// <summary>
/// Represents the type of individual or entity who received a passenger at dropoff.
/// </summary>
public enum HandOffRecipientType
{
    /// <summary>
    /// Passenger was handed off to their legal guardian.
    /// </summary>
    Guardian,

    /// <summary>
    /// Passenger was handed off to facility or medical staff.
    /// </summary>
    Staff,

    /// <summary>
    /// Passenger was able to self-discharge without assistance.
    /// </summary>
    Self,

    /// <summary>
    /// Passenger was handed off to another type of recipient.
    /// </summary>
    Other
}
