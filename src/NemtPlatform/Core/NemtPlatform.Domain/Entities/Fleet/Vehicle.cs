namespace NemtPlatform.Domain.Entities.Fleet;

using NemtPlatform.Domain.Common;
using NemtPlatform.Domain.Common.Enums;
using NemtPlatform.Domain.Common.ValueObjects;

/// <summary>
/// Represents a vehicle in the fleet used for transportation services.
/// Contains vehicle identification, capacity, compliance status, and medical capabilities.
/// </summary>
public class Vehicle : TenantEntity
{
    /// <summary>
    /// Gets or sets the display name of the vehicle.
    /// Required field used for identification (e.g., "Van 12", "Sedan 5").
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the operational status of the vehicle.
    /// Determines whether the vehicle is available for service, in maintenance, or decommissioned.
    /// </summary>
    public VehicleStatus Status { get; set; } = VehicleStatus.Active;

    /// <summary>
    /// Gets or sets the current compliance status of the vehicle.
    /// Indicates if all regulatory requirements and certifications are current.
    /// </summary>
    public ComplianceStatus CurrentComplianceStatus { get; set; } = ComplianceStatus.Clear;

    /// <summary>
    /// Gets or sets the Vehicle Identification Number (VIN).
    /// Optional unique identifier from the manufacturer.
    /// </summary>
    public string? Vin { get; set; }

    /// <summary>
    /// Gets or sets the license plate number.
    /// Required for vehicle registration and identification.
    /// </summary>
    public string LicensePlate { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the vehicle manufacturer.
    /// Optional field (e.g., "Ford", "Chevrolet", "Toyota").
    /// </summary>
    public string? Make { get; set; }

    /// <summary>
    /// Gets or sets the vehicle model.
    /// Optional field (e.g., "Transit", "Silverado", "Sienna").
    /// </summary>
    public string? Model { get; set; }

    /// <summary>
    /// Gets or sets the model year of the vehicle.
    /// Optional field used for maintenance scheduling and depreciation.
    /// </summary>
    public int? Year { get; set; }

    /// <summary>
    /// Gets or sets the foreign key to the home Region for this vehicle.
    /// Optional identifier indicating the primary service area for the vehicle.
    /// </summary>
    public string? HomeRegionId { get; set; }

    /// <summary>
    /// Gets or sets the type of vehicle.
    /// Determines the general category (Sedan, Van, WheelchairVan, SUV).
    /// </summary>
    public VehicleType VehicleType { get; set; }

    /// <summary>
    /// Gets or sets the capacity profile for the vehicle.
    /// Required value object defining wheelchair spaces, ambulatory seats, and stretcher capacity.
    /// </summary>
    public CapacityRequirements CapacityProfile { get; set; } = null!;

    /// <summary>
    /// Gets or sets the medical capabilities of the vehicle.
    /// Optional value object indicating the level of medical service (BLS, ALS, CCT) and onboard equipment.
    /// Null if the vehicle is not equipped for medical transport.
    /// </summary>
    public MedicalCapabilities? MedicalCapabilities { get; set; }

    /// <summary>
    /// Gets or sets the list of special vehicle attributes or features.
    /// Optional list of tags (e.g., "POWER_INVERTER", "BARIATRIC_RATED", "GPS_TRACKING").
    /// </summary>
    public List<string>? VehicleAttributes { get; set; }
}
