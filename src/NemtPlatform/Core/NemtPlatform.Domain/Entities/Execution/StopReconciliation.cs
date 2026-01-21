namespace NemtPlatform.Domain.Entities.Execution;

using NemtPlatform.Domain.Common.Enums;
using NemtPlatform.Domain.Common.ValueObjects;

/// <summary>
/// Represents the verified ground truth of what occurred at a specific stop during trip execution.
/// This is the authoritative record used for billing, payroll, and compliance reporting.
/// Immutable record to ensure data integrity for financial and legal purposes.
/// </summary>
/// <param name="StopId">The identifier of the stop that was executed.</param>
/// <param name="Outcome">The final outcome of the stop attempt (completed, no-show, etc.).</param>
/// <param name="ActualCapacityDelta">The actual capacity change at this stop (may differ from planned).</param>
/// <param name="Timestamp">The exact date and time when the stop outcome was recorded.</param>
/// <param name="VerifiedBy">The identifier of the driver or user who verified the stop outcome.</param>
/// <param name="VerificationMethod">The method used to verify the stop (visual, photo, signature, scan).</param>
/// <param name="PhotoUrl">The URL to photographic evidence, if photo verification was used. Default is null.</param>
/// <param name="SignatureData">The base64-encoded signature data, if signature verification was used. Default is null.</param>
/// <param name="ScannedData">The scanned QR code, barcode, or NFC tag data, if scan verification was used. Default is null.</param>
/// <param name="HandOffRecipient">The recipient who received the passenger at dropoff. Default is null.</param>
/// <param name="DriverNotes">Optional notes from the driver about the stop. Default is null.</param>
public record StopReconciliation(
    string StopId,
    StopOutcome Outcome,
    CapacityRequirements ActualCapacityDelta,
    DateTimeOffset Timestamp,
    string VerifiedBy,
    ReconciliationMethod VerificationMethod,
    string? PhotoUrl = null,
    string? SignatureData = null,
    ScannedData? ScannedData = null,
    HandOffRecipient? HandOffRecipient = null,
    string? DriverNotes = null);
