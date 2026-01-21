namespace NemtPlatform.Domain.Entities.Billing;

using NemtPlatform.Domain.Common;

/// <summary>
/// Represents a partner organization that refers passengers for transportation services.
/// Examples include hospitals, clinics, school districts, or care facilities.
/// </summary>
public class Partner : TenantEntity
{
    /// <summary>
    /// Gets or sets the name of the partner organization.
    /// Required field identifying the partner.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the list of foreign keys to FundingSource entities that this partner is authorized to use.
    /// Defines which funding sources the partner can bill for passenger transportation.
    /// </summary>
    public List<string> AuthorizedFundingSourceIds { get; set; } = new();
}
