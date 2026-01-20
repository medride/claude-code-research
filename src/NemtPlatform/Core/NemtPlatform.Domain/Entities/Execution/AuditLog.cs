namespace NemtPlatform.Domain.Entities.Execution;

using NemtPlatform.Domain.Common;
using NemtPlatform.Domain.Common.Enums;

/// <summary>
/// Represents a single entry in the system audit trail for compliance and security monitoring.
/// Captures all significant actions, data changes, and access events for regulatory compliance (HIPAA, SOC 2).
/// Immutable after creation to ensure audit trail integrity.
/// </summary>
public class AuditLog : TenantEntity
{
    /// <summary>
    /// Gets or sets the type of entity that was affected by this action.
    /// Required field. Examples: "Passenger", "Trip", "Vehicle", "User".
    /// </summary>
    public string EntityType { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the identifier of the specific entity instance that was affected.
    /// Required field. Allows drilling down to the exact record that changed.
    /// </summary>
    public string EntityId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the type of action that was performed.
    /// Required field. Categorizes the audit event for filtering and reporting.
    /// </summary>
    public AuditAction Action { get; set; }

    /// <summary>
    /// Gets or sets the JSON-formatted diff showing what changed.
    /// Null for actions that don't involve data changes (e.g., Login, Access).
    /// Should contain before/after values for Update actions.
    /// </summary>
    public string? Changes { get; set; }

    /// <summary>
    /// Gets or sets the exact timestamp when the action occurred.
    /// Required field. Critical for timeline reconstruction and compliance reporting.
    /// </summary>
    public DateTimeOffset Timestamp { get; set; }

    /// <summary>
    /// Gets or sets the foreign key to the User who performed the action.
    /// Null for system-initiated actions or unauthenticated access attempts.
    /// </summary>
    public string? UserId { get; set; }

    /// <summary>
    /// Gets or sets the role of the user at the time the action was performed.
    /// Null if no user context exists. Stored separately to track role changes over time.
    /// </summary>
    public string? UserRole { get; set; }

    /// <summary>
    /// Gets or sets the IP address from which the action was initiated.
    /// Null for server-side or system-initiated actions. Used for security analysis.
    /// </summary>
    public string? IpAddress { get; set; }

    /// <summary>
    /// Gets or sets additional context or notes about the audit event.
    /// Null for standard operations. May contain reason codes or justification for sensitive actions.
    /// </summary>
    public string? Notes { get; set; }
}
