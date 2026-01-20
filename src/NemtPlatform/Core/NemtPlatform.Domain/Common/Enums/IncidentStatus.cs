namespace NemtPlatform.Domain.Common.Enums;

/// <summary>
/// Represents the current status of an incident.
/// </summary>
public enum IncidentStatus
{
    /// <summary>
    /// Incident has been reported but not yet addressed.
    /// </summary>
    Reported,

    /// <summary>
    /// Incident is currently being addressed.
    /// </summary>
    InProgress,

    /// <summary>
    /// Incident has been resolved but not yet closed.
    /// </summary>
    Resolved,

    /// <summary>
    /// Incident has been closed and archived.
    /// </summary>
    Closed
}
