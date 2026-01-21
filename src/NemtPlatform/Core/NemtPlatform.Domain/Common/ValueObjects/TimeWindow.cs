namespace NemtPlatform.Domain.Common.ValueObjects;

/// <summary>
/// Represents a flexible time constraint window with optional minimum and maximum start/end times.
/// Used for scheduling trips and appointments with time flexibility.
/// </summary>
/// <param name="MinStartTime">The earliest acceptable start time. Null indicates no minimum constraint.</param>
/// <param name="MaxStartTime">The latest acceptable start time. Null indicates no maximum constraint.</param>
/// <param name="MinEndTime">The earliest acceptable end time. Null indicates no minimum constraint.</param>
/// <param name="MaxEndTime">The latest acceptable end time. Null indicates no maximum constraint.</param>
public record TimeWindow(
    TimeOnly? MinStartTime,
    TimeOnly? MaxStartTime,
    TimeOnly? MinEndTime,
    TimeOnly? MaxEndTime);
