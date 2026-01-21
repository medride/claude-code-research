namespace NemtPlatform.Domain.Common.ValueObjects;

/// <summary>
/// Represents data captured from scanning a QR code, barcode, or NFC tag during stop reconciliation.
/// </summary>
public record ScannedData
{
    /// <summary>
    /// The type of scan performed (QR code, barcode, or NFC tag).
    /// </summary>
    public ScanType Type { get; init; }

    /// <summary>
    /// The scanned value or identifier.
    /// </summary>
    public string Value { get; init; }

    /// <summary>
    /// Parameterless constructor for EF Core and JSON serialization.
    /// </summary>
    public ScannedData()
    {
        Value = string.Empty;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ScannedData"/> class.
    /// </summary>
    public ScannedData(ScanType type, string value)
    {
        Type = type;
        Value = value;
    }
}

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
