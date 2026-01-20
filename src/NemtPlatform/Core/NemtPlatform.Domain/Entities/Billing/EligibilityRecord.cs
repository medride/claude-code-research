namespace NemtPlatform.Domain.Entities.Billing;

using NemtPlatform.Domain.Common;
using NemtPlatform.Domain.Common.Enums;

/// <summary>
/// Represents a historical record of a passenger's coverage eligibility with a funding source.
/// Tracks when the passenger was eligible for services and when eligibility was verified.
/// </summary>
public class EligibilityRecord : TenantEntity
{
    /// <summary>
    /// Gets or sets the foreign key to the Passenger whose eligibility is recorded.
    /// Required field linking the record to the passenger.
    /// </summary>
    public string PassengerId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the foreign key to the FundingSource for which eligibility is being tracked.
    /// Required field identifying which funding source's coverage is being tracked.
    /// </summary>
    public string FundingSourceId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the eligibility status at the time of this record.
    /// Indicates whether the passenger was active, inactive, or pending verification.
    /// </summary>
    public EligibilityStatus Status { get; set; }

    /// <summary>
    /// Gets or sets the date when this eligibility period started.
    /// Required field marking the beginning of the coverage period.
    /// </summary>
    public DateTimeOffset EffectiveStartDate { get; set; }

    /// <summary>
    /// Gets or sets the date when this eligibility period ended.
    /// Required field marking the end of the coverage period.
    /// </summary>
    public DateTimeOffset EffectiveEndDate { get; set; }

    /// <summary>
    /// Gets or sets the timestamp when this eligibility was last verified with the funding source.
    /// Required field for audit and compliance purposes.
    /// </summary>
    public DateTimeOffset LastVerifiedAt { get; set; }
}
