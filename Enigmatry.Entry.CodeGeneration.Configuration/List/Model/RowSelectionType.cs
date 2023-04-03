namespace Enigmatry.Entry.CodeGeneration.Configuration.List.Model;

/// <summary>
/// The RowSelectionType enumeration defines the available row selection modes for a table component.
/// It is used to configure how rows can be selected within a table (no selection, single or multi select).
/// </summary>
public enum RowSelectionType
{
    /// <summary>
    /// No row selection is allowed.
    /// </summary>
    None,
    /// <summary>
    /// Only a single row can be selected at a time.
    /// </summary>
    Single,
    /// <summary>
    /// Multiple rows can be selected at once.
    /// </summary>
    Multiple
}
