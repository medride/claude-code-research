namespace NemtPlatform.Domain.Common.ValueObjects;

/// <summary>
/// Represents a destination that is pre-authorized for a passenger's transportation.
/// Used in authorization records to specify approved locations.
/// </summary>
/// <param name="PlaceId">The foreign key to the Place entity representing the authorized destination.</param>
/// <param name="DocumentationUrl">Optional URL to documentation supporting this authorization.</param>
/// <param name="Notes">Optional notes about this authorized destination.</param>
public record AuthorizedDestination(
    string PlaceId,
    string? DocumentationUrl = null,
    string? Notes = null);
