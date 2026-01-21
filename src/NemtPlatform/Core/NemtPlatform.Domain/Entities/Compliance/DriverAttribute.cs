namespace NemtPlatform.Domain.Entities.Compliance;

using NemtPlatform.Domain.Common;
using NemtPlatform.Domain.Common.Enums;

/// <summary>
/// Represents an instance of a non-licensed attribute held by a driver.
/// This entity tracks specific qualifications or compliance requirements that a driver has completed,
/// such as background checks, drug screenings, or training programs.
/// </summary>
public class DriverAttribute : TenantEntity
{
    /// <summary>
    /// Gets or sets the foreign key to the DriverProfile entity.
    /// Required link to the driver who holds this attribute.
    /// </summary>
    public string DriverProfileId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the foreign key to the AttributeDefinition.
    /// Required link to the type of attribute (e.g., "Background Check", "Drug Test").
    /// </summary>
    public string AttributeId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the status of the attribute.
    /// Indicates whether the attribute is pending verification, active, expired, or revoked.
    /// </summary>
    public CredentialStatus Status { get; set; } = CredentialStatus.PendingVerification;

    /// <summary>
    /// Gets or sets the date when the attribute was awarded or completed.
    /// Required timestamp indicating when the driver completed this requirement.
    /// </summary>
    public DateTimeOffset DateAwarded { get; set; }

    /// <summary>
    /// Gets or sets the expiration date of the attribute.
    /// Optional field for attributes that require periodic renewal.
    /// Null indicates the attribute does not expire (e.g., one-time background check).
    /// </summary>
    public DateTimeOffset? ExpirationDate { get; set; }

    /// <summary>
    /// Gets or sets the foreign key to the Document entity.
    /// Optional link to the digital copy of the attribute documentation or certificate.
    /// </summary>
    public string? DocumentId { get; set; }
}
