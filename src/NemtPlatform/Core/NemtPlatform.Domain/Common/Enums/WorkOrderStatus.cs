namespace NemtPlatform.Domain.Common.Enums;

/// <summary>
/// Represents the status of a maintenance work order.
/// </summary>
public enum WorkOrderStatus
{
    /// <summary>
    /// Work order has been requested but not yet scheduled.
    /// </summary>
    Requested,

    /// <summary>
    /// Work order has been scheduled for a specific date and time.
    /// </summary>
    Scheduled,

    /// <summary>
    /// Work is currently in progress.
    /// </summary>
    InProgress,

    /// <summary>
    /// Work order has been completed.
    /// </summary>
    Completed,

    /// <summary>
    /// Work order has been canceled.
    /// </summary>
    Canceled
}
