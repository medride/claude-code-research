namespace NemtPlatform.Domain.Entities.Billing;

using NemtPlatform.Domain.Common;

/// <summary>
/// Represents an organization that pays for transportation services.
/// Examples include insurance companies, Medicaid, Medicare, or school boards.
/// </summary>
public class FundingSource : TenantEntity
{
    /// <summary>
    /// Gets or sets the name of the funding source organization.
    /// Required field identifying the payer.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the foreign key to the ProcedureSet that defines required procedures for this funding source.
    /// If set, specifies which procedures must be followed when billing this funding source.
    /// </summary>
    public string? ProcedureSetId { get; set; }
}
