namespace NemtPlatform.Domain.Entities.Billing;

using NemtPlatform.Domain.Common;
using NemtPlatform.Domain.Common.Enums;

/// <summary>
/// Represents a standardized billing code used for healthcare services.
/// Includes HCPCS, CPT, ICD-10, and modifier codes.
/// </summary>
public class ServiceCode : TenantEntity
{
    /// <summary>
    /// Gets or sets the service code value (e.g., "A0130" for wheelchair van transport).
    /// Required field containing the actual billing code.
    /// </summary>
    public string Code { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the human-readable description of what this service code represents.
    /// Required field explaining the service or procedure.
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the type of service code (HCPCS, CPT, ICD-10, or Modifier).
    /// Indicates which coding system this code belongs to.
    /// </summary>
    public ServiceCodeType Type { get; set; }

    /// <summary>
    /// Gets or sets the default billing rate for this service code.
    /// Optional field; actual rates may vary by contract or funding source.
    /// </summary>
    public decimal? DefaultRate { get; set; }
}
