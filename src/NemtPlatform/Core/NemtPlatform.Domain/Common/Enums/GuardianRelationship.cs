namespace NemtPlatform.Domain.Common.Enums;

/// <summary>
/// Defines the relationship type between a guardian and a passenger.
/// Used for legal and operational purposes when managing passenger care.
/// </summary>
public enum GuardianRelationship
{
    /// <summary>
    /// The guardian is a biological or adoptive parent.
    /// </summary>
    Parent,

    /// <summary>
    /// The guardian has legal guardianship through court order or similar legal mechanism.
    /// </summary>
    LegalGuardian,

    /// <summary>
    /// The guardian is a social worker or case manager assigned to the passenger.
    /// </summary>
    Caseworker,

    /// <summary>
    /// The relationship does not fit into the predefined categories.
    /// </summary>
    Other
}
