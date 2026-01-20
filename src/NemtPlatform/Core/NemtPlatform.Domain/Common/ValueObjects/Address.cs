namespace NemtPlatform.Domain.Common.ValueObjects;

/// <summary>
/// Represents a physical mailing address.
/// </summary>
/// <param name="Street">The street address including number and street name.</param>
/// <param name="City">The city name.</param>
/// <param name="State">The state or province code (e.g., "CA", "NY").</param>
/// <param name="ZipCode">The postal/ZIP code.</param>
public record Address(string Street, string City, string State, string ZipCode);
