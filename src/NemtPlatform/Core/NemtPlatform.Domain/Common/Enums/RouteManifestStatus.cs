namespace NemtPlatform.Domain.Common.Enums;

/// <summary>
/// Represents the status of a RouteManifest in the trip planning and execution lifecycle.
/// </summary>
public enum RouteManifestStatus
{
    /// <summary>
    /// The manifest is being planned but not yet assigned to a shift or driver.
    /// </summary>
    Planning,

    /// <summary>
    /// The manifest has been dispatched to a driver and is ready for execution.
    /// </summary>
    Dispatched,

    /// <summary>
    /// All trips in the manifest have been completed.
    /// </summary>
    Completed
}
