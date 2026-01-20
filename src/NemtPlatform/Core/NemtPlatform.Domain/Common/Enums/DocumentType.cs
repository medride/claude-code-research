namespace NemtPlatform.Domain.Common.Enums;

/// <summary>
/// Represents the type of document in the system.
/// </summary>
public enum DocumentType
{
    /// <summary>
    /// Vehicle registration document.
    /// </summary>
    Registration,

    /// <summary>
    /// Insurance policy or certificate.
    /// </summary>
    Insurance,

    /// <summary>
    /// Invoice or billing document.
    /// </summary>
    Invoice,

    /// <summary>
    /// Vehicle or property title document.
    /// </summary>
    Title,

    /// <summary>
    /// Photographic evidence or documentation.
    /// </summary>
    Photo,

    /// <summary>
    /// Other document type not categorized above.
    /// </summary>
    Other
}
