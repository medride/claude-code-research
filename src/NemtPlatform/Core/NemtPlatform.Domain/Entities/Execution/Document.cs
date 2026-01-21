namespace NemtPlatform.Domain.Entities.Execution;

using NemtPlatform.Domain.Common;
using NemtPlatform.Domain.Common.Enums;

/// <summary>
/// Represents a stored file or document associated with an entity in the system.
/// Used for maintaining references to regulatory documents, photos, signatures, and other files.
/// Stores metadata and URL reference; actual file storage is handled by external blob storage.
/// </summary>
public class Document : TenantEntity
{
    /// <summary>
    /// Gets or sets the original filename of the uploaded document.
    /// Required field. Preserves the original file name for user reference.
    /// </summary>
    public string FileName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the URL or storage path where the document is stored.
    /// Required field. Typically points to blob storage (e.g., Azure Blob Storage, S3).
    /// </summary>
    public string FileUrl { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the category of document for organization and access control.
    /// Required field. Determines handling, retention policies, and compliance requirements.
    /// </summary>
    public DocumentType DocumentType { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the entity this document is associated with.
    /// Required field. Links the document to a specific record (vehicle, trip, employee, etc.).
    /// </summary>
    public string AssociatedEntityId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the type of entity this document is associated with.
    /// Required field. Specifies whether the document belongs to a vehicle, trip, employee, etc.
    /// </summary>
    public AssociatedEntityType AssociatedEntityType { get; set; }

    /// <summary>
    /// Gets or sets the timestamp when the document was uploaded to the system.
    /// Required field. Used for audit trails and document lifecycle management.
    /// </summary>
    public DateTimeOffset UploadDate { get; set; }

    /// <summary>
    /// Gets or sets the foreign key to the User who uploaded the document.
    /// Null for system-generated or migrated documents.
    /// </summary>
    public string? UploadedByUserId { get; set; }

    /// <summary>
    /// Gets or sets optional notes or description about the document.
    /// Null if no additional context is needed. May include validation status or special instructions.
    /// </summary>
    public string? Notes { get; set; }
}
