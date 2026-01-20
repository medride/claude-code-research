namespace NemtPlatform.Domain.Entities.Billing;

using NemtPlatform.Domain.Common;
using NemtPlatform.Domain.Common.Enums;
using NemtPlatform.Domain.Common.ValueObjects;

/// <summary>
/// Represents an approval from a funding source for a passenger to receive transportation services.
/// Defines what services are authorized, for which destinations, and any usage limits.
/// </summary>
public class Authorization : TenantEntity
{
    /// <summary>
    /// Gets or sets the foreign key to the Passenger who is authorized for services.
    /// Required field linking the authorization to the recipient.
    /// </summary>
    public string PassengerId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the foreign key to the FundingSource that issued this authorization.
    /// Required field identifying who is authorizing and paying for the services.
    /// </summary>
    public string FundingSourceId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the current status of the authorization.
    /// Indicates whether the authorization is active, expired, exhausted, or canceled.
    /// </summary>
    public AuthorizationStatus Status { get; set; }

    /// <summary>
    /// Gets or sets the authorization code or number provided by the funding source.
    /// Required field used for billing and verification purposes.
    /// </summary>
    public string AuthorizationCode { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the date range during which this authorization is valid.
    /// Required field defining when services can be provided under this authorization.
    /// </summary>
    public DateRange EffectiveDateRange { get; set; } = null!;

    /// <summary>
    /// Gets or sets the list of destinations that are authorized for transportation.
    /// If set, limits transportation to these specific locations.
    /// </summary>
    public List<AuthorizedDestination>? AuthorizedDestinations { get; set; }

    /// <summary>
    /// Gets or sets the usage limits for this authorization.
    /// Defines maximum trips, miles, or costs allowed.
    /// </summary>
    public AuthorizationLimits? Limits { get; set; }

    /// <summary>
    /// Gets or sets the list of approved service codes that can be billed under this authorization.
    /// Foreign keys to ServiceCode entities.
    /// </summary>
    public List<string>? ApprovedServiceCodes { get; set; }
}
