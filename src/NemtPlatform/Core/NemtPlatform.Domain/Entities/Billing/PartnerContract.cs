namespace NemtPlatform.Domain.Entities.Billing;

using NemtPlatform.Domain.Common;
using NemtPlatform.Domain.Common.Enums;
using NemtPlatform.Domain.Common.ValueObjects;

/// <summary>
/// Represents a formal contract between the transportation provider and a partner organization.
/// Defines service terms, pricing, and service level agreements.
/// </summary>
public class PartnerContract : TenantEntity
{
    /// <summary>
    /// Gets or sets the foreign key to the Partner organization.
    /// Required field linking the contract to the partner.
    /// </summary>
    public string PartnerId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the name or title of the contract.
    /// Required field for identifying and referencing the contract.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the current status of the contract.
    /// Indicates whether the contract is in draft, active, expired, or terminated.
    /// </summary>
    public PartnerContractStatus Status { get; set; }

    /// <summary>
    /// Gets or sets the date range during which this contract is effective.
    /// Required field defining the start and end dates of the contract term.
    /// </summary>
    public DateRange EffectiveDateRange { get; set; } = null!;

    /// <summary>
    /// Gets or sets the service level agreement terms for this contract.
    /// Defines performance targets and maximum wait times.
    /// </summary>
    public ServiceLevelAgreement? Sla { get; set; }
}
