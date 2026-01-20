namespace NemtPlatform.Domain.Common.ValueObjects;

/// <summary>
/// Represents specific notification triggers for trip-related events.
/// Used within GuardianPermissions to control which events trigger notifications.
/// </summary>
/// <param name="OnPickup">Whether to send notifications when the passenger is picked up.</param>
/// <param name="OnDropoff">Whether to send notifications when the passenger is dropped off.</param>
/// <param name="OnDelay">Whether to send notifications when a trip is delayed.</param>
public record NotificationSettings(bool OnPickup, bool OnDropoff, bool OnDelay);
