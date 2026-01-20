namespace NemtPlatform.Domain.Common.ValueObjects;

/// <summary>
/// Represents a single item in an inspection checklist.
/// </summary>
/// <param name="Category">The category or system this checklist item belongs to (e.g., "Brakes", "Lights", "Safety Equipment").</param>
/// <param name="Prompt">The specific instruction or question to check (e.g., "Check brake pedal resistance", "Verify all lights are operational").</param>
public record ChecklistItem(
    string Category,
    string Prompt);
