namespace NemtPlatform.Infrastructure.MultiTenancy;

/// <summary>
/// Strategy for resolving tenant ID from the current request.
/// </summary>
public interface ITenantResolver
{
    /// <summary>
    /// Attempts to resolve the tenant ID from the current context.
    /// </summary>
    /// <returns>
    /// A task that represents the asynchronous operation.
    /// The task result contains the tenant ID if resolved, otherwise null.
    /// </returns>
    Task<string?> ResolveAsync();
}
