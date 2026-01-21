namespace NemtPlatform.Domain.Common.ValueObjects;

/// <summary>
/// Represents the usage limits for a passenger's transportation authorization.
/// Defines maximum allowed trips, miles, or costs.
/// </summary>
public record AuthorizationLimits
{
    /// <summary>
    /// The maximum number of trips allowed under this authorization.
    /// </summary>
    public int? MaxTrips { get; init; }

    /// <summary>
    /// The maximum number of miles allowed under this authorization.
    /// </summary>
    public int? MaxMiles { get; init; }

    /// <summary>
    /// The maximum cost allowed under this authorization.
    /// </summary>
    public decimal? MaxCost { get; init; }

    /// <summary>
    /// Parameterless constructor for EF Core and JSON serialization.
    /// </summary>
    public AuthorizationLimits()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="AuthorizationLimits"/> class.
    /// </summary>
    public AuthorizationLimits(int? maxTrips = null, int? maxMiles = null, decimal? maxCost = null)
    {
        MaxTrips = maxTrips;
        MaxMiles = maxMiles;
        MaxCost = maxCost;
    }
}
