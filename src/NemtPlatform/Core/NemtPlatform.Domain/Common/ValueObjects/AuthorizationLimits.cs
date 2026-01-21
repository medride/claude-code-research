namespace NemtPlatform.Domain.Common.ValueObjects;

/// <summary>
/// Represents the usage limits for a passenger's transportation authorization.
/// Defines maximum allowed trips, miles, or costs.
/// </summary>
/// <param name="MaxTrips">The maximum number of trips allowed under this authorization.</param>
/// <param name="MaxMiles">The maximum number of miles allowed under this authorization.</param>
/// <param name="MaxCost">The maximum cost allowed under this authorization.</param>
public record AuthorizationLimits(
    int? MaxTrips = null,
    int? MaxMiles = null,
    decimal? MaxCost = null);
