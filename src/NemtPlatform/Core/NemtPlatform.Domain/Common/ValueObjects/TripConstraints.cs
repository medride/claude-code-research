namespace NemtPlatform.Domain.Common.ValueObjects;

using NemtPlatform.Domain.Common.Enums;

/// <summary>
/// Defines the shape of constraints that apply to a Driver.
/// </summary>
public record DriverConstraints
{
    /// <summary>Specific driver IDs that match this constraint (instance-based).</summary>
    public List<string>? Ids { get; init; }

    /// <summary>Required gender of the driver.</summary>
    public Gender? Gender { get; init; }

    /// <summary>List of AttributeDefinition IDs the driver MUST possess (hard requirement for scheduling).</summary>
    public List<string>? RequiredAttributes { get; init; }

    /// <summary>
    /// Parameterless constructor for EF Core.
    /// </summary>
    public DriverConstraints() { }

    /// <summary>
    /// Creates a new DriverConstraints with the specified values.
    /// </summary>
    public DriverConstraints(
        List<string>? ids = null,
        Gender? gender = null,
        List<string>? requiredAttributes = null)
    {
        Ids = ids;
        Gender = gender;
        RequiredAttributes = requiredAttributes;
    }
}

/// <summary>
/// Defines the shape of constraints that apply to a Vehicle.
/// </summary>
public record VehicleConstraints
{
    /// <summary>Specific vehicle IDs that match this constraint (instance-based).</summary>
    public List<string>? Ids { get; init; }

    /// <summary>Required type of vehicle.</summary>
    public VehicleType? Type { get; init; }

    /// <summary>
    /// Parameterless constructor for EF Core.
    /// </summary>
    public VehicleConstraints() { }

    /// <summary>
    /// Creates a new VehicleConstraints with the specified values.
    /// </summary>
    public VehicleConstraints(List<string>? ids = null, VehicleType? type = null)
    {
        Ids = ids;
        Type = type;
    }
}

/// <summary>
/// A set of constraints for a driver and a vehicle.
/// </summary>
public record ConstraintSet
{
    /// <summary>Constraints that apply to the driver assignment.</summary>
    public DriverConstraints? Driver { get; init; }

    /// <summary>Constraints that apply to the vehicle assignment.</summary>
    public VehicleConstraints? Vehicle { get; init; }

    /// <summary>
    /// Parameterless constructor for EF Core.
    /// </summary>
    public ConstraintSet() { }

    /// <summary>
    /// Creates a new ConstraintSet with the specified values.
    /// </summary>
    public ConstraintSet(DriverConstraints? driver = null, VehicleConstraints? vehicle = null)
    {
        Driver = driver;
        Vehicle = vehicle;
    }
}

/// <summary>
/// The main container for all trip constraints, categorized by their strictness.
/// This structure is essential for optimization engines to find valid and optimal assignments.
/// </summary>
public record TripConstraints
{
    /// <summary>SOFT constraints - try to match these if possible but not required.</summary>
    public ConstraintSet? Preferences { get; init; }

    /// <summary>HARD constraints - the assignment MUST match these attributes.</summary>
    public ConstraintSet? Requirements { get; init; }

    /// <summary>HARD constraints - do NOT assign if these match.</summary>
    public ConstraintSet? Prohibitions { get; init; }

    /// <summary>
    /// Parameterless constructor for EF Core.
    /// </summary>
    public TripConstraints() { }

    /// <summary>
    /// Creates a new TripConstraints with the specified values.
    /// </summary>
    public TripConstraints(
        ConstraintSet? preferences = null,
        ConstraintSet? requirements = null,
        ConstraintSet? prohibitions = null)
    {
        Preferences = preferences;
        Requirements = requirements;
        Prohibitions = prohibitions;
    }
}
