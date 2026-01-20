namespace NemtPlatform.Domain.Common.ValueObjects;

using NemtPlatform.Domain.Common.Enums;

/// <summary>
/// Represents the medical capabilities of a vehicle.
/// Defines the level of medical service and available onboard equipment.
/// </summary>
/// <param name="LevelOfService">The medical service level the vehicle can provide (BLS, ALS, or CCT).</param>
/// <param name="OnboardEquipmentIds">Optional list of identifiers for onboard medical equipment.</param>
public record MedicalCapabilities(
    MedicalServiceLevel LevelOfService,
    List<string>? OnboardEquipmentIds = null);
