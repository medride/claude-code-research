namespace NemtPlatform.Domain.Common.Enums;

/// <summary>
/// Represents the type of action performed on an entity for audit trail purposes.
/// Used for compliance, security monitoring, and change tracking.
/// </summary>
public enum AuditAction
{
    /// <summary>
    /// A new entity was created.
    /// </summary>
    Create,

    /// <summary>
    /// An existing entity was modified.
    /// </summary>
    Update,

    /// <summary>
    /// An entity was deleted.
    /// </summary>
    Delete,

    /// <summary>
    /// An entity was accessed or viewed.
    /// </summary>
    Access,

    /// <summary>
    /// User logged into the system.
    /// </summary>
    Login,

    /// <summary>
    /// User logged out of the system.
    /// </summary>
    Logout,

    /// <summary>
    /// User password was changed.
    /// </summary>
    PasswordChange,

    /// <summary>
    /// User role or permissions were changed.
    /// </summary>
    RoleChange,

    /// <summary>
    /// Data was exported from the system.
    /// </summary>
    DataExport,

    /// <summary>
    /// Data was imported into the system.
    /// </summary>
    DataImport,

    /// <summary>
    /// Permissions were modified.
    /// </summary>
    PermissionChange,

    /// <summary>
    /// Other action not categorized above.
    /// </summary>
    Other
}
