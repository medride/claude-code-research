namespace NemtPlatform.Domain.Entities.Execution;

using NemtPlatform.Domain.Common.Enums;
using NemtPlatform.Domain.Common.ValueObjects;

/// <summary>
/// Represents the verified ground truth of what occurred at a specific stop during trip execution.
/// This is the authoritative record used for billing, payroll, and compliance reporting.
/// Immutable record to ensure data integrity for financial and legal purposes.
/// </summary>
public record StopReconciliation
{
    /// <summary>The identifier of the stop that was executed.</summary>
    public string StopId { get; init; } = string.Empty;

    /// <summary>The final outcome of the stop attempt (completed, no-show, etc.).</summary>
    public StopOutcome Outcome { get; init; }

    /// <summary>The actual capacity change at this stop (may differ from planned).</summary>
    public CapacityRequirements? ActualCapacityDelta { get; init; }

    /// <summary>The exact date and time when the stop outcome was recorded.</summary>
    public DateTimeOffset Timestamp { get; init; }

    /// <summary>The identifier of the driver or user who verified the stop outcome.</summary>
    public string VerifiedBy { get; init; } = string.Empty;

    /// <summary>The method used to verify the stop (visual, photo, signature, scan).</summary>
    public ReconciliationMethod VerificationMethod { get; init; }

    /// <summary>The URL to photographic evidence, if photo verification was used.</summary>
    public string? PhotoUrl { get; init; }

    /// <summary>The base64-encoded signature data, if signature verification was used.</summary>
    public string? SignatureData { get; init; }

    /// <summary>The scanned QR code, barcode, or NFC tag data, if scan verification was used.</summary>
    public ScannedData? ScannedData { get; init; }

    /// <summary>The recipient who received the passenger at dropoff.</summary>
    public HandOffRecipient? HandOffRecipient { get; init; }

    /// <summary>Optional notes from the driver about the stop.</summary>
    public string? DriverNotes { get; init; }

    /// <summary>Parameterless constructor for EF Core and JSON serialization.</summary>
    public StopReconciliation() { }

    /// <summary>Creates a new StopReconciliation with the specified values.</summary>
    public StopReconciliation(
        string stopId,
        StopOutcome outcome,
        CapacityRequirements actualCapacityDelta,
        DateTimeOffset timestamp,
        string verifiedBy,
        ReconciliationMethod verificationMethod,
        string? photoUrl = null,
        string? signatureData = null,
        ScannedData? scannedData = null,
        HandOffRecipient? handOffRecipient = null,
        string? driverNotes = null)
    {
        StopId = stopId;
        Outcome = outcome;
        ActualCapacityDelta = actualCapacityDelta;
        Timestamp = timestamp;
        VerifiedBy = verifiedBy;
        VerificationMethod = verificationMethod;
        PhotoUrl = photoUrl;
        SignatureData = signatureData;
        ScannedData = scannedData;
        HandOffRecipient = handOffRecipient;
        DriverNotes = driverNotes;
    }
}
