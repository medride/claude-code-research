namespace NemtPlatform.Domain.Common.ValueObjects;

/// <summary>
/// Represents the financial breakdown of maintenance work costs.
/// </summary>
/// <param name="Parts">Cost of replacement parts and materials.</param>
/// <param name="Labor">Cost of labor for the maintenance work.</param>
/// <param name="Tax">Tax amount applied to the maintenance work.</param>
/// <param name="Total">Total cost including parts, labor, and tax.</param>
public record MaintenanceCosts(
    decimal Parts,
    decimal Labor,
    decimal Tax,
    decimal Total);
