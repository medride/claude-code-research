namespace NemtPlatform.Domain.Common.ValueObjects;

/// <summary>
/// Represents a single line item in a billing claim.
/// Each line item corresponds to a specific service code and charge amount.
/// </summary>
public record ClaimLineItem
{
    /// <summary>
    /// The unique identifier for this line item.
    /// </summary>
    public string Id { get; init; }

    /// <summary>
    /// The foreign key to the ServiceCode being billed.
    /// </summary>
    public string ServiceCodeId { get; init; }

    /// <summary>
    /// The amount being charged for this service.
    /// </summary>
    public decimal ChargeAmount { get; init; }

    /// <summary>
    /// The number of units of service being billed.
    /// </summary>
    public int Units { get; init; }

    /// <summary>
    /// Optional list of modifier codes that adjust the base service code.
    /// </summary>
    public List<string>? Modifiers { get; init; }

    /// <summary>
    /// Parameterless constructor for EF Core and JSON serialization.
    /// </summary>
    public ClaimLineItem()
    {
        Id = string.Empty;
        ServiceCodeId = string.Empty;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ClaimLineItem"/> class.
    /// </summary>
    public ClaimLineItem(string id, string serviceCodeId, decimal chargeAmount, int units, List<string>? modifiers = null)
    {
        Id = id;
        ServiceCodeId = serviceCodeId;
        ChargeAmount = chargeAmount;
        Units = units;
        Modifiers = modifiers;
    }
}
