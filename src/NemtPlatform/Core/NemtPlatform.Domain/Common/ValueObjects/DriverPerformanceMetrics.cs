namespace NemtPlatform.Domain.Common.ValueObjects;

/// <summary>
/// Represents key performance indicators for a driver's operational performance.
/// </summary>
/// <param name="OnTimePercentage">The percentage of trips completed on time (0-100).</param>
/// <param name="PassengerSatisfactionRating">The average passenger satisfaction rating on a scale of 1-5 stars.</param>
/// <param name="IncidentFreeMiles">The total number of miles driven without incidents.</param>
public record DriverPerformanceMetrics(
    decimal OnTimePercentage,
    decimal PassengerSatisfactionRating,
    int IncidentFreeMiles);
