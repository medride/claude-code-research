namespace NemtPlatform.Domain.Common.ValueObjects;

/// <summary>
/// Represents a physical mailing address.
/// </summary>
public record Address
{
    /// <summary>
    /// The street address including number and street name.
    /// </summary>
    public string Street { get; init; }

    /// <summary>
    /// The city name.
    /// </summary>
    public string City { get; init; }

    /// <summary>
    /// The state or province code (e.g., "CA", "NY").
    /// </summary>
    public string State { get; init; }

    /// <summary>
    /// The postal/ZIP code.
    /// </summary>
    public string ZipCode { get; init; }

    /// <summary>
    /// Parameterless constructor for EF Core and JSON serialization.
    /// </summary>
    public Address()
    {
        Street = string.Empty;
        City = string.Empty;
        State = string.Empty;
        ZipCode = string.Empty;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Address"/> class.
    /// </summary>
    public Address(string street, string city, string state, string zipCode)
    {
        Street = street;
        City = city;
        State = state;
        ZipCode = zipCode;
    }
}
