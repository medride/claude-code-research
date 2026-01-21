namespace NemtPlatform.Domain.Common.ValueObjects;

/// <summary>
/// Represents service level agreement metrics for partner contracts.
/// Defines performance targets and service quality requirements.
/// </summary>
public record ServiceLevelAgreement
{
    /// <summary>
    /// The target percentage for on-time performance (0.0 to 1.0).
    /// </summary>
    public decimal OnTimePerformanceTarget { get; init; }

    /// <summary>
    /// The maximum acceptable wait time in minutes.
    /// </summary>
    public int MaxWaitTimeMinutes { get; init; }

    /// <summary>
    /// Parameterless constructor for EF Core and JSON serialization.
    /// </summary>
    public ServiceLevelAgreement()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ServiceLevelAgreement"/> class.
    /// </summary>
    public ServiceLevelAgreement(decimal onTimePerformanceTarget, int maxWaitTimeMinutes)
    {
        OnTimePerformanceTarget = onTimePerformanceTarget;
        MaxWaitTimeMinutes = maxWaitTimeMinutes;
    }
}
