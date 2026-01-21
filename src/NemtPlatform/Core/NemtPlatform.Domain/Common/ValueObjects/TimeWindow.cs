namespace NemtPlatform.Domain.Common.ValueObjects;

/// <summary>
/// Represents a flexible time constraint window with optional minimum and maximum start/end times.
/// Used for scheduling trips and appointments with time flexibility.
/// </summary>
public record TimeWindow
{
    /// <summary>
    /// The earliest acceptable start time. Null indicates no minimum constraint.
    /// </summary>
    public TimeOnly? MinStartTime { get; init; }

    /// <summary>
    /// The latest acceptable start time. Null indicates no maximum constraint.
    /// </summary>
    public TimeOnly? MaxStartTime { get; init; }

    /// <summary>
    /// The earliest acceptable end time. Null indicates no minimum constraint.
    /// </summary>
    public TimeOnly? MinEndTime { get; init; }

    /// <summary>
    /// The latest acceptable end time. Null indicates no maximum constraint.
    /// </summary>
    public TimeOnly? MaxEndTime { get; init; }

    /// <summary>
    /// Parameterless constructor for EF Core and JSON serialization.
    /// </summary>
    public TimeWindow()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="TimeWindow"/> class.
    /// </summary>
    public TimeWindow(
        TimeOnly? minStartTime,
        TimeOnly? maxStartTime,
        TimeOnly? minEndTime,
        TimeOnly? maxEndTime)
    {
        MinStartTime = minStartTime;
        MaxStartTime = maxStartTime;
        MinEndTime = minEndTime;
        MaxEndTime = maxEndTime;
    }
}
