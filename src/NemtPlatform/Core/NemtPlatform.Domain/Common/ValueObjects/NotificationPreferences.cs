namespace NemtPlatform.Domain.Common.ValueObjects;

using NemtPlatform.Domain.Common.Enums;

/// <summary>
/// Represents user preferences for receiving notifications.
/// Defines communication channels and optional quiet hours.
/// </summary>
/// <param name="Channels">The list of notification channels the user wants to receive notifications through.</param>
/// <param name="QuietHours">Optional time range during which notifications should not be sent.</param>
public record NotificationPreferences(List<NotificationChannel> Channels, QuietHours? QuietHours = null);
