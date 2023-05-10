using System.Collections.Generic;
using System.Linq;
using Humanizer;

namespace Enigmatry.Entry.CodeGeneration.Configuration.List.Model;

/// <summary>
/// The RowInfoBuilder class provides a fluent API for configuring row-specific settings for a table component.
/// </summary>
/// <remarks>
/// <para>
/// It offers methods to define the row selection type and to configure the row context menu.<br/>
/// Use this builder when defining a table component to customize the behavior and appearance of table rows.
/// </para>
/// <para>
/// This builder is accessible through the <see cref="ListComponentBuilder{T}.Row">Row()</see> method of the <see cref="ListComponentBuilder{T}"/>.
/// </para>
/// </remarks>
public class RowInfoBuilder
{
    private RowSelectionType _selectionType = RowSelectionType.None;
    private bool _showSelectAllOption = true;
    private IEnumerable<RowContextMenuItem> _contextMenuItems = new List<RowContextMenuItem>();
    private bool _showContextMenu = false;

    /// <summary>
    /// Set the row selection type for the table component.
    /// </summary>
    /// <param name="selectionType">
    /// The row selection type: <see cref="RowSelectionType.None">None</see>, <see cref="RowSelectionType.Single">Single</see>, or <see cref="RowSelectionType.Multiple">Multiple</see>.
    /// </param>
    /// <returns>The RowInfoBuilder instance for method chaining.</returns>
    public RowInfoBuilder Selection(RowSelectionType selectionType)
    {
        _selectionType = selectionType;
        return this;
    }

    /// <summary>
    /// Set the row selection type for the table component.
    /// </summary>
    /// <param name="selectionType">
    /// The row selection type: <see cref="RowSelectionType.None">None</see>,
    /// <see cref="RowSelectionType.Single">Single</see>, or <see cref="RowSelectionType.Multiple">Multiple</see>.
    /// </param>
    /// <param name="showSelectAllOption">Optional, whether to include a select all checkbox.</param>
    /// <returns>The RowInfoBuilder instance for method chaining.</returns>
    public RowInfoBuilder Selection(RowSelectionType selectionType, bool showSelectAllOption)
    {
        _selectionType = selectionType;
        _showSelectAllOption = showSelectAllOption;
        return this;
    }

    /// <summary>
    /// Set whether to show the context menu on row selection.
    /// </summary>
    /// <param name="show">True if context menu should be shown, false otherwise.</param>
    /// <returns>The RowInfoBuilder instance for method chaining.</returns>
    public RowInfoBuilder ShowContextMenu(bool show)
    {
        _showContextMenu = show;
        return this;
    }

    /// <summary>
    /// Set the context menu items for the table component.
    /// </summary>
    /// <param name="items">An array of strings representing the context menu items.</param>
    /// <returns>The RowInfoBuilder instance for method chaining.</returns>
    public RowInfoBuilder ContextMenuItems(params string[] items)
    {
        Check.NotNull(items, nameof(items));

        _contextMenuItems = items.Select(item => new RowContextMenuItem { Id = item.Kebaberize(), Name = item.Humanize() });
        return this;
    }

    /// <summary>
    /// Set the context menu items for the table component.
    /// </summary>
    /// <param name="items">An array of <see cref="RowContextMenuItem" /> objects representing the context menu items.</param>
    /// <returns>The RowInfoBuilder instance for method chaining.</returns>
    public RowInfoBuilder ContextMenuItems(params RowContextMenuItem[] items)
    {
        Check.NotNull(items, nameof(items));

        _contextMenuItems = items.ToList();
        return this;
    }

    public RowInfo Build(ComponentInfo componentInfo)
    {
        foreach (var contextMenuItem in _contextMenuItems)
        {
            contextMenuItem.TranslationId ??= $"{componentInfo.TranslationId}.context.{contextMenuItem.Id.Kebaberize()}";
        }
        return new RowInfo
        {
            Selection = _selectionType,
            ShowSelectAllOption = _showSelectAllOption,
            ShowContextMenu = _showContextMenu,
            ContextMenuItems = _contextMenuItems,
            ComponentInfo = componentInfo
        };
    }
}
