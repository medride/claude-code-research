namespace NemtPlatform.Domain.Common.ValueObjects;

/// <summary>
/// Represents specific notification triggers for trip-related events.
/// Used within GuardianPermissions to control which events trigger notifications.
/// </summary>
public record NotificationSettings
{
    /// <summary>Whether to send notifications when the passenger is picked up.</summary>
    public bool OnPickup { get; init; }

    /// <summary>Whether to send notifications when the passenger is dropped off.</summary>
    public bool OnDropoff { get; init; }

    /// <summary>Whether to send notifications when a trip is delayed.</summary>
    public bool OnDelay { get; init; }

    /// <summary>Parameterless constructor for EF Core and JSON serialization.</summary>
    public NotificationSettings() { }

    /// <summary>Creates a new NotificationSettings with the specified values.</summary>
    public NotificationSettings(bool onPickup, bool onDropoff, bool onDelay)
    {
        OnPickup = onPickup;
        OnDropoff = onDropoff;
        OnDelay = onDelay;
    }
}
