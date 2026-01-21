namespace NemtPlatform.Domain.Common.ValueObjects;

using NemtPlatform.Domain.Common.Enums;

/// <summary>
/// Represents the medical capabilities of a vehicle.
/// Defines the level of medical service and available onboard equipment.
/// </summary>
public record MedicalCapabilities
{
    /// <summary>
    /// The medical service level the vehicle can provide (BLS, ALS, or CCT).
    /// </summary>
    public MedicalServiceLevel LevelOfService { get; init; }

    /// <summary>
    /// Optional list of identifiers for onboard medical equipment.
    /// </summary>
    public List<string>? OnboardEquipmentIds { get; init; }

    /// <summary>
    /// Parameterless constructor for EF Core and JSON serialization.
    /// </summary>
    public MedicalCapabilities()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="MedicalCapabilities"/> class.
    /// </summary>
    public MedicalCapabilities(MedicalServiceLevel levelOfService, List<string>? onboardEquipmentIds = null)
    {
        LevelOfService = levelOfService;
        OnboardEquipmentIds = onboardEquipmentIds;
    }
}
