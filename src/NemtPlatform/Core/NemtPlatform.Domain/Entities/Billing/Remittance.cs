namespace NemtPlatform.Domain.Entities.Billing;

using NemtPlatform.Domain.Common;
using NemtPlatform.Domain.Common.ValueObjects;

/// <summary>
/// Represents a payment record received from a funding source.
/// Documents the payment amount, associated claims, and any adjustments applied.
/// </summary>
public class Remittance : TenantEntity
{
    /// <summary>
    /// Gets or sets the foreign key to the FundingSource that issued this payment.
    /// Required field identifying the payer.
    /// </summary>
    public string FundingSourceId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the date when the payment was made or received.
    /// Required field for financial tracking and reconciliation.
    /// </summary>
    public DateTimeOffset PaymentDate { get; set; }

    /// <summary>
    /// Gets or sets the total amount paid in this remittance.
    /// Required field representing the payment received.
    /// </summary>
    public decimal TotalPaidAmount { get; set; }

    /// <summary>
    /// Gets or sets the check number or electronic payment reference number.
    /// Required field for payment tracking and reconciliation.
    /// </summary>
    public string CheckOrReferenceNumber { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the list of foreign keys to Claim entities covered by this payment.
    /// Required field linking the payment to the claims being paid.
    /// </summary>
    public List<string> ClaimIds { get; set; } = new();

    /// <summary>
    /// Gets or sets the list of adjustments applied to claim line items.
    /// Documents any reductions, denials, or modifications to the original claim amounts.
    /// </summary>
    public List<RemittanceAdjustment>? Adjustments { get; set; }
}
