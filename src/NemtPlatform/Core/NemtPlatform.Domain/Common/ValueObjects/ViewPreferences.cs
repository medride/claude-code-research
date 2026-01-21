namespace NemtPlatform.Domain.Common.ValueObjects;

/// <summary>
/// Represents user-specific view preferences for a data table or list.
/// Used to persist column visibility, ordering, sizing, and sorting preferences.
/// </summary>
public record ViewPreferences
{
    /// <summary>
    /// Ordered list of column identifiers.
    /// </summary>
    public List<string>? ColumnOrder { get; init; }

    /// <summary>
    /// Dictionary mapping column IDs to visibility state (true = visible).
    /// </summary>
    public Dictionary<string, bool>? ColumnVisibility { get; init; }

    /// <summary>
    /// Dictionary mapping column IDs to pixel widths.
    /// </summary>
    public Dictionary<string, int>? ColumnSizes { get; init; }

    /// <summary>
    /// List of active sorting states for columns.
    /// </summary>
    public List<SortingState>? Sorting { get; init; }

    /// <summary>
    /// Parameterless constructor for EF Core and JSON serialization.
    /// </summary>
    public ViewPreferences()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ViewPreferences"/> class.
    /// </summary>
    public ViewPreferences(
        List<string>? columnOrder = null,
        Dictionary<string, bool>? columnVisibility = null,
        Dictionary<string, int>? columnSizes = null,
        List<SortingState>? sorting = null)
    {
        ColumnOrder = columnOrder;
        ColumnVisibility = columnVisibility;
        ColumnSizes = columnSizes;
        Sorting = sorting;
    }
}

/// <summary>
/// Represents the sorting state for a single column.
/// </summary>
public record SortingState
{
    /// <summary>
    /// The column identifier.
    /// </summary>
    public string Id { get; init; }

    /// <summary>
    /// True if sorted in descending order, false for ascending.
    /// </summary>
    public bool Descending { get; init; }

    /// <summary>
    /// Parameterless constructor for EF Core and JSON serialization.
    /// </summary>
    public SortingState()
    {
        Id = string.Empty;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SortingState"/> class.
    /// </summary>
    public SortingState(string id, bool descending)
    {
        Id = id;
        Descending = descending;
    }
}
