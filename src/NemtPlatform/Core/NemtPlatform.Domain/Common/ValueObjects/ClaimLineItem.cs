namespace NemtPlatform.Domain.Common.ValueObjects;

/// <summary>
/// Represents a single line item in a billing claim.
/// Each line item corresponds to a specific service code and charge amount.
/// </summary>
/// <param name="Id">The unique identifier for this line item.</param>
/// <param name="ServiceCodeId">The foreign key to the ServiceCode being billed.</param>
/// <param name="ChargeAmount">The amount being charged for this service.</param>
/// <param name="Units">The number of units of service being billed.</param>
/// <param name="Modifiers">Optional list of modifier codes that adjust the base service code.</param>
public record ClaimLineItem(
    string Id,
    string ServiceCodeId,
    decimal ChargeAmount,
    int Units,
    List<string>? Modifiers = null);
