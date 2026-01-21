namespace NemtPlatform.Domain.Common.ValueObjects;

/// <summary>
/// Represents a destination that is pre-authorized for a passenger's transportation.
/// Used in authorization records to specify approved locations.
/// </summary>
public record AuthorizedDestination
{
    /// <summary>
    /// The foreign key to the Place entity representing the authorized destination.
    /// </summary>
    public string PlaceId { get; init; }

    /// <summary>
    /// Optional URL to documentation supporting this authorization.
    /// </summary>
    public string? DocumentationUrl { get; init; }

    /// <summary>
    /// Optional notes about this authorized destination.
    /// </summary>
    public string? Notes { get; init; }

    /// <summary>
    /// Parameterless constructor for EF Core and JSON serialization.
    /// </summary>
    public AuthorizedDestination()
    {
        PlaceId = string.Empty;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="AuthorizedDestination"/> class.
    /// </summary>
    public AuthorizedDestination(string placeId, string? documentationUrl = null, string? notes = null)
    {
        PlaceId = placeId;
        DocumentationUrl = documentationUrl;
        Notes = notes;
    }
}
