namespace NemtPlatform.Domain.Common.Enums;

/// <summary>
/// Defines the context in which a form is being used.
/// This determines which fields are shown, required, or editable.
/// </summary>
public enum FormContext
{
    /// <summary>
    /// Creating a new entity.
    /// </summary>
    Create,

    /// <summary>
    /// Editing an existing entity.
    /// </summary>
    Edit,

    /// <summary>
    /// Viewing an existing entity (read-only).
    /// </summary>
    View,

    /// <summary>
    /// Filtering or searching for entities.
    /// </summary>
    Filter,

    /// <summary>
    /// Inline form (embedded in a table or list).
    /// </summary>
    Inline,

    /// <summary>
    /// Modal dialog form.
    /// </summary>
    Modal
}
