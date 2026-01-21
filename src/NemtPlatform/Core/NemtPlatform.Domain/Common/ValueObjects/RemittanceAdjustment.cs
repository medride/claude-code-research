namespace NemtPlatform.Domain.Common.ValueObjects;

/// <summary>
/// Represents an adjustment applied to a claim line item during payment processing.
/// Used in remittance records to track payment modifications, denials, or reductions.
/// </summary>
public record RemittanceAdjustment
{
    /// <summary>
    /// The identifier of the claim line item being adjusted.
    /// </summary>
    public string LineItemId { get; init; }

    /// <summary>
    /// The code indicating the reason for this adjustment.
    /// </summary>
    public string ReasonCode { get; init; }

    /// <summary>
    /// The adjustment amount (positive for additions, negative for deductions).
    /// </summary>
    public decimal Amount { get; init; }

    /// <summary>
    /// Optional human-readable description of the adjustment.
    /// </summary>
    public string? Description { get; init; }

    /// <summary>
    /// Parameterless constructor for EF Core and JSON serialization.
    /// </summary>
    public RemittanceAdjustment()
    {
        LineItemId = string.Empty;
        ReasonCode = string.Empty;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="RemittanceAdjustment"/> class.
    /// </summary>
    public RemittanceAdjustment(string lineItemId, string reasonCode, decimal amount, string? description = null)
    {
        LineItemId = lineItemId;
        ReasonCode = reasonCode;
        Amount = amount;
        Description = description;
    }
}
