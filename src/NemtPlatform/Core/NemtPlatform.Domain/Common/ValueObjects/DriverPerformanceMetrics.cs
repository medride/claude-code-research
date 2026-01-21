namespace NemtPlatform.Domain.Common.ValueObjects;

/// <summary>
/// Represents key performance indicators for a driver's operational performance.
/// </summary>
public record DriverPerformanceMetrics
{
    /// <summary>
    /// The percentage of trips completed on time (0-100).
    /// </summary>
    public decimal OnTimePercentage { get; init; }

    /// <summary>
    /// The average passenger satisfaction rating on a scale of 1-5 stars.
    /// </summary>
    public decimal PassengerSatisfactionRating { get; init; }

    /// <summary>
    /// The total number of miles driven without incidents.
    /// </summary>
    public int IncidentFreeMiles { get; init; }

    /// <summary>
    /// Parameterless constructor for EF Core and JSON serialization.
    /// </summary>
    public DriverPerformanceMetrics()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="DriverPerformanceMetrics"/> class.
    /// </summary>
    public DriverPerformanceMetrics(decimal onTimePercentage, decimal passengerSatisfactionRating, int incidentFreeMiles)
    {
        OnTimePercentage = onTimePercentage;
        PassengerSatisfactionRating = passengerSatisfactionRating;
        IncidentFreeMiles = incidentFreeMiles;
    }
}
