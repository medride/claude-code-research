namespace NemtPlatform.Domain.Entities.Operations;

using NemtPlatform.Domain.Common;

/// <summary>
/// Represents a single, continuous period of time a driver was online during a parent Shift.
/// A single Shift can have multiple sessions, for example if the driver accidentally clicks
/// to end their shift and then has to go back online.
/// </summary>
public class ShiftSession : TenantEntity
{
    /// <summary>
    /// Gets or sets the foreign key to the parent Shift.
    /// Required field.
    /// </summary>
    public string ShiftId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets when this specific session started.
    /// Required field.
    /// </summary>
    public DateTimeOffset StartTime { get; set; }

    /// <summary>
    /// Gets or sets when this session ended.
    /// Null if this is the currently active session.
    /// Optional field.
    /// </summary>
    public DateTimeOffset? EndTime { get; set; }
}
