namespace NemtPlatform.Domain.Entities.Fleet;

using NemtPlatform.Domain.Common;
using NemtPlatform.Domain.Common.Enums;

/// <summary>
/// Represents a time-sensitive credential or certification for a vehicle.
/// Used for tracking registrations, insurance policies, inspections, and other required documentation.
/// </summary>
public class VehicleCredential : TenantEntity
{
    /// <summary>
    /// Gets or sets the foreign key to the Vehicle entity.
    /// Required link to the vehicle this credential applies to.
    /// </summary>
    public string VehicleId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the foreign key to the Credential definition.
    /// Required link to the type of credential (e.g., "Registration", "Insurance", "Inspection").
    /// </summary>
    public string CredentialId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the status of the credential.
    /// Indicates whether the credential is pending, active, expired, or revoked.
    /// </summary>
    public CredentialStatus Status { get; set; } = CredentialStatus.PendingVerification;

    /// <summary>
    /// Gets or sets the policy number or document number for this credential.
    /// Optional identifier from the issuing authority or insurance provider.
    /// </summary>
    public string? PolicyOrDocumentNumber { get; set; }

    /// <summary>
    /// Gets or sets the date when the credential was issued.
    /// Optional field for record-keeping and verification.
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
