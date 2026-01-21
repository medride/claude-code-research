namespace NemtPlatform.Domain.Common.ValueObjects;

/// <summary>
/// Represents the permissions and notification settings for a guardian relationship.
/// Controls what actions a guardian can perform on behalf of a passenger.
/// </summary>
public record GuardianPermissions
{
    /// <summary>Whether the guardian can schedule, modify, or cancel trips for the passenger.</summary>
    public bool CanManageSchedule { get; init; }

    /// <summary>Whether the guardian can view and manage billing information.</summary>
    public bool CanManageBilling { get; init; }

    /// <summary>Whether the guardian can view historical trip and medical records.</summary>
    public bool CanViewHistory { get; init; }

    /// <summary>Whether this guardian is the primary contact for the passenger.</summary>
    public bool IsPrimaryContact { get; init; }

    /// <summary>Optional settings for trip-related notifications.</summary>
    public NotificationSettings? NotificationSettings { get; init; }

    /// <summary>Parameterless constructor for EF Core and JSON serialization.</summary>
    public GuardianPermissions() { }

    /// <summary>Creates a new GuardianPermissions with the specified values.</summary>
    public GuardianPermissions(
        bool canManageSchedule,
        bool canManageBilling,
        bool canViewHistory,
        bool isPrimaryContact,
        NotificationSettings? notificationSettings = null)
    {
        CanManageSchedule = canManageSchedule;
        CanManageBilling = canManageBilling;
        CanViewHistory = canViewHistory;
        IsPrimaryContact = isPrimaryContact;
        NotificationSettings = notificationSettings;
    }
}
