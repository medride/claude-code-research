namespace NemtPlatform.Domain.Entities.Fleet;

using NemtPlatform.Domain.Common;

/// <summary>
/// Represents a fuel transaction record for a vehicle.
/// Used for expense tracking, fuel efficiency analysis, and mileage reporting.
/// </summary>
public class FuelLog : TenantEntity
{
    /// <summary>
    /// Gets or sets the foreign key to the Vehicle that was fueled.
    /// Required link to the vehicle receiving fuel.
    /// </summary>
    public string VehicleId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the foreign key to the Driver who performed the fueling.
    /// Optional link to the employee who fueled the vehicle.
    /// </summary>
    public string? DriverId { get; set; }

    /// <summary>
    /// Gets or sets the date and time of the fuel transaction.
    /// Required timestamp for expense reporting and analysis.
    /// </summary>
    public DateTimeOffset TransactionDate { get; set; }

    /// <summary>
    /// Gets or sets the odometer reading at the time of fueling.
    /// Used for calculating fuel efficiency and tracking vehicle usage.
    /// </summary>
    public int OdometerReading { get; set; }

    /// <summary>
    /// Gets or sets the number of gallons purchased.
    /// Quantity of fuel added to the vehicle.
    /// </summary>
    public decimal Gallons { get; set; }

    /// <summary>
    /// Gets or sets the cost per gallon of fuel.
    /// Unit price for the fuel at the time of purchase.
    /// </summary>
    public decimal CostPerGallon { get; set; }

    /// <summary>
    /// Gets or sets the total cost of the fuel transaction.
    /// Total amount paid for the fuel purchase.
    /// </summary>
    public decimal TotalCost { get; set; }

    /// <summary>
    /// Gets or sets the name of the fuel vendor or station.
    /// Optional field for tracking preferred vendors and locations.
    /// </summary>
    public string? VendorName { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the fuel card used for the transaction.
    /// Optional field for reconciling fuel card statements.
    /// </summary>
    public string? FuelCardId { get; set; }
}
