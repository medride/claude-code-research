namespace NemtPlatform.Domain.Common.Enums;

/// <summary>
/// Defines the available channels for sending notifications to users.
/// Used in notification preference settings for guardians and other stakeholders.
/// </summary>
public enum NotificationChannel
{
    /// <summary>
    /// Send notifications via SMS text message.
    /// </summary>
    Sms,

    /// <summary>
    /// Send notifications via email.
    /// </summary>
    Email,

    /// <summary>
    /// Send notifications via mobile push notification.
    /// </summary>
    Push
}
