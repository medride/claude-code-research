namespace NemtPlatform.Domain.Entities.Billing;

using NemtPlatform.Domain.Common;
using NemtPlatform.Domain.Common.Enums;
using NemtPlatform.Domain.Common.ValueObjects;

/// <summary>
/// Represents a billing claim submitted to a funding source for reimbursement.
/// Contains line items for services rendered and tracks submission and payment status.
/// </summary>
public class Claim : TenantEntity
{
    /// <summary>
    /// Gets or sets the list of foreign keys to Trip entities being billed in this claim.
    /// Required field linking the claim to the transportation services provided.
    /// </summary>
    public List<string> TripIds { get; set; } = new();

    /// <summary>
    /// Gets or sets the foreign key to the FundingSource receiving this claim.
    /// Required field identifying the payer.
    /// </summary>
    public string FundingSourceId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the current status of the claim.
    /// Indicates whether the claim is draft, submitted, paid, partially paid, denied, or pending information.
    /// </summary>
    public ClaimStatus Status { get; set; }

    /// <summary>
    /// Gets or sets the foreign key to the Partner organization associated with this claim.
    /// Optional field used when billing through a partner contract.
    /// </summary>
    public string? PartnerId { get; set; }

    /// <summary>
    /// Gets or sets the foreign key to the PartnerContract governing this claim.
    /// Optional field used when specific contract terms apply.
    /// </summary>
    public string? ContractId { get; set; }

    /// <summary>
    /// Gets or sets the list of individual service line items being billed.
    /// Required field containing the detailed breakdown of charges.
    /// </summary>
    public List<ClaimLineItem> LineItems { get; set; } = new();

    /// <summary>
    /// Gets or sets the list of foreign keys to diagnosis codes (ICD-10) supporting this claim.
    /// Used for medical necessity documentation.
    /// </summary>
    public List<string> DiagnosisCodeIds { get; set; } = new();

    /// <summary>
    /// Gets or sets the total amount being billed in this claim.
    /// Required field representing the sum of all line item charges.
    /// </summary>
    public decimal TotalBilledAmount { get; set; }

    /// <summary>
    /// Gets or sets the timestamp when this claim was submitted to the funding source.
    /// Null if the claim has not yet been submitted.
    /// </summary>
    public DateTimeOffset? DateSubmitted { get; set; }
}
