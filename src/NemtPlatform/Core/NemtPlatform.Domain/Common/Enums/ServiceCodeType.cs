namespace NemtPlatform.Domain.Common.Enums;

/// <summary>
/// Represents the type of service code used for billing purposes.
/// </summary>
public enum ServiceCodeType
{
    /// <summary>
    /// Healthcare Common Procedure Coding System code.
    /// </summary>
    Hcpcs,

    /// <summary>
    /// Current Procedural Terminology code.
    /// </summary>
    Cpt,

    /// <summary>
    /// International Classification of Diseases, 10th Revision code.
    /// </summary>
    Icd10,

    /// <summary>
    /// Billing modifier code that adjusts the base service code.
    /// </summary>
    Modifier
}
