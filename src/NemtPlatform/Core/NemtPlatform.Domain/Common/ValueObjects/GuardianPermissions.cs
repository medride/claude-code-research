namespace NemtPlatform.Domain.Common.ValueObjects;

/// <summary>
/// Represents the permissions and notification settings for a guardian relationship.
/// Controls what actions a guardian can perform on behalf of a passenger.
/// </summary>
/// <param name="CanManageSchedule">Whether the guardian can schedule, modify, or cancel trips for the passenger.</param>
/// <param name="CanManageBilling">Whether the guardian can view and manage billing information.</param>
/// <param name="CanViewHistory">Whether the guardian can view historical trip and medical records.</param>
/// <param name="IsPrimaryContact">Whether this guardian is the primary contact for the passenger.</param>
/// <param name="NotificationSettings">Optional settings for trip-related notifications.</param>
public record GuardianPermissions(
    bool CanManageSchedule,
    bool CanManageBilling,
    bool CanViewHistory,
    bool IsPrimaryContact,
    NotificationSettings? NotificationSettings = null);
