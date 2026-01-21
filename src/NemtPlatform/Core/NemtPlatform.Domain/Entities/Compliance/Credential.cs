namespace NemtPlatform.Domain.Entities.Compliance;

using NemtPlatform.Domain.Common;

/// <summary>
/// Represents a master definition of a credential or license type.
/// This is a system-wide entity that defines the type of credentials that can be issued,
/// such as driver's licenses, CPR certifications, or medical certifications.
/// Not tenant-specific as it represents universal credential definitions.
/// </summary>
public class Credential : Entity
{
    /// <summary>
    /// Gets or sets the name of the credential type.
    /// Required field identifying the credential (e.g., "Commercial Driver's License", "CPR Certification").
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the description of the credential.
    /// Optional field providing additional details about the credential's purpose and requirements.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Gets or sets the issuing body or authority for this credential.
    /// Optional field identifying the organization that issues this credential
    /// (e.g., "State of California", "NREMT", "American Red Cross").
    /// </summary>
    public string? IssuingBody { get; set; }
}
