namespace NemtPlatform.Domain.Common.Enums;

/// <summary>
/// Represents the method used to verify and reconcile a stop outcome.
/// Used for compliance, billing accuracy, and dispute resolution.
/// </summary>
public enum ReconciliationMethod
{
    /// <summary>
    /// The stop outcome was verified through visual inspection by the driver.
    /// </summary>
    Visual,

    /// <summary>
    /// The stop outcome was verified with photographic evidence.
    /// </summary>
    Photo,

    /// <summary>
    /// The stop outcome was verified with a signature from the passenger or guardian.
    /// </summary>
    Signature,

    /// <summary>
    /// The stop outcome was verified by scanning a QR code, barcode, or NFC tag.
    /// </summary>
    Scan
}
