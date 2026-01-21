namespace NemtPlatform.Domain.Common.ValueObjects;

using NemtPlatform.Domain.Common.Enums;

/// <summary>
/// Represents user preferences for receiving notifications.
/// Defines communication channels and optional quiet hours.
/// </summary>
public record NotificationPreferences
{
    /// <summary>The list of notification channels the user wants to receive notifications through.</summary>
    public List<NotificationChannel> Channels { get; init; } = new();

    /// <summary>Optional time range during which notifications should not be sent.</summary>
    public QuietHours? QuietHours { get; init; }

    /// <summary>Parameterless constructor for EF Core and JSON serialization.</summary>
    public NotificationPreferences() { }

    /// <summary>Creates a new NotificationPreferences with the specified values.</summary>
    public NotificationPreferences(List<NotificationChannel> channels, QuietHours? quietHours = null)
    {
        Channels = channels;
        QuietHours = quietHours;
    }
}
