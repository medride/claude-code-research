namespace NemtPlatform.Domain.Entities.Compliance;

using NemtPlatform.Domain.Common;
using NemtPlatform.Domain.Common.Enums;

/// <summary>
/// Represents a specific credential held by an employee.
/// This entity tracks the instance of a credential type assigned to a particular employee,
/// including its status, expiration, and associated documentation.
/// </summary>
public class EmployeeCredential : TenantEntity
{
    /// <summary>
    /// Gets or sets the foreign key to the Employee entity.
    /// Required link to the employee who holds this credential.
    /// </summary>
    public string EmployeeId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the foreign key to the Credential definition.
    /// Required link to the type of credential (e.g., "CDL Class B", "EMT-B License").
    /// </summary>
    public string CredentialId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the status of the credential.
    /// Indicates whether the credential is pending verification, active, expired, or revoked.
    /// </summary>
    public CredentialStatus Status { get; set; } = CredentialStatus.PendingVerification;

    /// <summary>
    /// Gets or sets the license or certification number.
    /// Optional identifier assigned by the issuing authority.
    /// </summary>
    public string? LicenseNumber { get; set; }

    /// <summary>
    /// Gets or sets the date when the credential was issued.
    /// Optional field for record-keeping and verification purposes.
    /// </summary>
    public DateTimeOffset? IssueDate { get; set; }

    /// <summary>
    /// Gets or sets the expiration date of the credential.
    /// Required field used to track when the credential needs renewal.
    /// </summary>
    public DateTimeOffset ExpirationDate { get; set; }

    /// <summary>
    /// Gets or sets the foreign key to the Document entity.
    /// Optional link to the digital copy of the credential document.
    /// </summary>
    public string? DocumentId { get; set; }
}
