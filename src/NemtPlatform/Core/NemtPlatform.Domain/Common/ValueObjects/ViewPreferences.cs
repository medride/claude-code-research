namespace NemtPlatform.Domain.Common.ValueObjects;

/// <summary>
/// Represents user-specific view preferences for a data table or list.
/// Used to persist column visibility, ordering, sizing, and sorting preferences.
/// </summary>
/// <param name="ColumnOrder">Ordered list of column identifiers.</param>
/// <param name="ColumnVisibility">Dictionary mapping column IDs to visibility state (true = visible).</param>
/// <param name="ColumnSizes">Dictionary mapping column IDs to pixel widths.</param>
/// <param name="Sorting">List of active sorting states for columns.</param>
public record ViewPreferences(
    List<string>? ColumnOrder = null,
    Dictionary<string, bool>? ColumnVisibility = null,
    Dictionary<string, int>? ColumnSizes = null,
    List<SortingState>? Sorting = null);

/// <summary>
/// Represents the sorting state for a single column.
/// </summary>
/// <param name="Id">The column identifier.</param>
/// <param name="Descending">True if sorted in descending order, false for ascending.</param>
public record SortingState(
    string Id,
    bool Descending);
