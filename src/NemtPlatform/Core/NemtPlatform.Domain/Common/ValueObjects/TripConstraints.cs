namespace NemtPlatform.Domain.Common.ValueObjects;

using NemtPlatform.Domain.Common.Enums;

/// <summary>
/// Defines the shape of constraints that apply to a Driver.
/// </summary>
/// <param name="Ids">Specific driver IDs that match this constraint (instance-based).</param>
/// <param name="Gender">Required gender of the driver.</param>
/// <param name="RequiredAttributes">List of AttributeDefinition IDs the driver MUST possess (hard requirement for scheduling).</param>
public record DriverConstraints(
    List<string>? Ids = null,
    Gender? Gender = null,
    List<string>? RequiredAttributes = null);

/// <summary>
/// Defines the shape of constraints that apply to a Vehicle.
/// </summary>
/// <param name="Ids">Specific vehicle IDs that match this constraint (instance-based).</param>
/// <param name="Type">Required type of vehicle.</param>
public record VehicleConstraints(
    List<string>? Ids = null,
    VehicleType? Type = null);

/// <summary>
/// A set of constraints for a driver and a vehicle.
/// </summary>
/// <param name="Driver">Constraints that apply to the driver assignment.</param>
/// <param name="Vehicle">Constraints that apply to the vehicle assignment.</param>
public record ConstraintSet(
    DriverConstraints? Driver = null,
    VehicleConstraints? Vehicle = null);

/// <summary>
/// The main container for all trip constraints, categorized by their strictness.
/// This structure is essential for optimization engines to find valid and optimal assignments.
/// </summary>
/// <param name="Preferences">SOFT constraints - try to match these if possible but not required.</param>
/// <param name="Requirements">HARD constraints - the assignment MUST match these attributes.</param>
/// <param name="Prohibitions">HARD constraints - do NOT assign if these match.</param>
public record TripConstraints(
    ConstraintSet? Preferences = null,
    ConstraintSet? Requirements = null,
    ConstraintSet? Prohibitions = null);
