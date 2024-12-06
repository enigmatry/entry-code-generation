namespace Enigmatry.Entry.CodeGeneration.Configuration.List.Model;

/// <summary>
/// The RowContextMenuItem class represents an individual context menu item for a row in a table component.
/// It contains properties to set the item's identifier, display name, icon, and translation ID.
/// </summary>
public class RowContextMenuItem
{
    /// <summary>
    /// Gets or sets the unique identifier for the context menu item.
    /// </summary>
    public string Id { get; set; } = String.Empty;
    /// <summary>
    /// Gets or sets the display name of the context menu item.
    /// </summary>
    public string Name { get; set; } = String.Empty;
    /// <summary>
    /// Gets or sets the icon for the context menu item. The value should match an existing icon name in the application's icon set.
    /// </summary>
    public string? Icon { get; set; }
    /// <summary>
    /// Gets or sets the translation ID for the context menu item's display name. This property is used when the application is configured to use Angular i18n.
    /// </summary>
    public string? TranslationId { get; set; }
}
