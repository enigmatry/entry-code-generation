using System.Collections.Generic;
using System.Linq;
using Humanizer;

namespace Enigmatry.Entry.CodeGeneration.Configuration.List.Model
{
    public class RowInfoBuilder
    {
        private RowSelectionType _selectionType = RowSelectionType.None;
        private bool _showSelectAllOption = true;
        private IEnumerable<RowContextMenuItem> _contextMenuItems = new List<RowContextMenuItem>();
        private bool _showContextMenu = false;

        public RowInfoBuilder Selection(RowSelectionType selectionType)
        {
            _selectionType = selectionType;
            return this;
        }

        public RowInfoBuilder Selection(RowSelectionType selectionType, bool showSelectAllOption)
        {
            _selectionType = selectionType;
            _showSelectAllOption = showSelectAllOption;
            return this;
        }

        public RowInfoBuilder ShowContextMenu(bool show)
        {
            _showContextMenu = show;
            return this;
        }

        public RowInfoBuilder ContextMenuItems(params string[] items)
        {
            Check.NotNull(items, nameof(items));

            _contextMenuItems = items.Select(item => new RowContextMenuItem { Id = item.Kebaberize(), Name = item.Humanize() });
            return this;
        }

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
}
