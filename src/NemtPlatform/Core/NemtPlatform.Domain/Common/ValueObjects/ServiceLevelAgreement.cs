namespace NemtPlatform.Domain.Common.ValueObjects;

/// <summary>
/// Represents service level agreement metrics for partner contracts.
/// Defines performance targets and service quality requirements.
/// </summary>
/// <param name="OnTimePerformanceTarget">The target percentage for on-time performance (0.0 to 1.0).</param>
/// <param name="MaxWaitTimeMinutes">The maximum acceptable wait time in minutes.</param>
public record ServiceLevelAgreement(
    decimal OnTimePerformanceTarget,
    int MaxWaitTimeMinutes);
