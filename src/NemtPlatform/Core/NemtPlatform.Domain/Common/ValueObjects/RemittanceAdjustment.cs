namespace NemtPlatform.Domain.Common.ValueObjects;

/// <summary>
/// Represents an adjustment applied to a claim line item during payment processing.
/// Used in remittance records to track payment modifications, denials, or reductions.
/// </summary>
/// <param name="LineItemId">The identifier of the claim line item being adjusted.</param>
/// <param name="ReasonCode">The code indicating the reason for this adjustment.</param>
/// <param name="Amount">The adjustment amount (positive for additions, negative for deductions).</param>
/// <param name="Description">Optional human-readable description of the adjustment.</param>
public record RemittanceAdjustment(
    string LineItemId,
    string ReasonCode,
    decimal Amount,
    string? Description = null);
