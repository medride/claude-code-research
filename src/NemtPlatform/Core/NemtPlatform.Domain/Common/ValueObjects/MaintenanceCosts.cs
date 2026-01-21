namespace NemtPlatform.Domain.Common.ValueObjects;

/// <summary>
/// Represents the financial breakdown of maintenance work costs.
/// </summary>
public record MaintenanceCosts
{
    /// <summary>
    /// Cost of replacement parts and materials.
    /// </summary>
    public decimal Parts { get; init; }

    /// <summary>
    /// Cost of labor for the maintenance work.
    /// </summary>
    public decimal Labor { get; init; }

    /// <summary>
    /// Tax amount applied to the maintenance work.
    /// </summary>
    public decimal Tax { get; init; }

    /// <summary>
    /// Total cost including parts, labor, and tax.
    /// </summary>
    public decimal Total { get; init; }

    /// <summary>
    /// Parameterless constructor for EF Core and JSON serialization.
    /// </summary>
    public MaintenanceCosts()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="MaintenanceCosts"/> class.
    /// </summary>
    public MaintenanceCosts(decimal parts, decimal labor, decimal tax, decimal total)
    {
        Parts = parts;
        Labor = labor;
        Tax = tax;
        Total = total;
    }
}
