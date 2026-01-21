namespace NemtPlatform.Domain.Common.ValueObjects;

/// <summary>
/// Represents data captured from scanning a QR code, barcode, or NFC tag during stop reconciliation.
/// </summary>
/// <param name="Type">The type of scan performed (QR code, barcode, or NFC tag).</param>
/// <param name="Value">The scanned value or identifier.</param>
public record ScannedData(
    ScanType Type,
    string Value);

/// <summary>
/// Represents the type of scanning technology used for stop verification.
/// </summary>
public enum ScanType
{
    /// <summary>
    /// A QR code was scanned.
    /// </summary>
    QrCode,

    /// <summary>
    /// A barcode was scanned.
    /// </summary>
    Barcode,

    /// <summary>
    /// An NFC tag was scanned.
    /// </summary>
    NfcTag
}
