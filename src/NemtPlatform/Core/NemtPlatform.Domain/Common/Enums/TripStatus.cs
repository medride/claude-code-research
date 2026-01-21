namespace NemtPlatform.Domain.Common.Enums;

/// <summary>
/// Defines the lifecycle states of a trip from initial request through final completion or cancellation.
/// </summary>
public enum TripStatus
{
    /// <summary>
    /// The trip has been requested by a user but not yet reviewed by staff.
    /// </summary>
    PendingApproval,

    /// <summary>
    /// The trip request has been reviewed and rejected.
    /// </summary>
    Rejected,

    /// <summary>
    /// The trip is approved and is waiting to be scheduled onto a route.
    /// </summary>
    Approved,

    /// <summary>
    /// The trip has been assigned to a driver's route for a specific day.
    /// </summary>
    Scheduled,

    /// <summary>
    /// The driver has begun executing the route this trip is on.
    /// </summary>
    InProgress,

    /// <summary>
    /// The trip has been fully completed (both pickup and dropoff).
    /// </summary>
    Completed,

    /// <summary>
    /// The trip was started but not completed successfully.
    /// </summary>
    Incomplete,

    /// <summary>
    /// The trip has been canceled.
    /// </summary>
    Canceled
}
