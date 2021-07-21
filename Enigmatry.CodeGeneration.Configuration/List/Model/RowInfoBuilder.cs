using System.Collections.Generic;
using System.Linq;
using Humanizer;

namespace Enigmatry.CodeGeneration.Configuration.List.Model
{
    public class RowInfoBuilder : IBuilder<RowInfo>
    {
        private RowSelectionType _selectionType = RowSelectionType.None;
        private bool _showContextMenu = false;
        private IEnumerable<RowContextMenuItem> _contextMenuItems = new List<RowContextMenuItem>();

        public RowInfoBuilder Selection(RowSelectionType selectionType)
        {
            _selectionType = selectionType;
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

            _contextMenuItems = items.Select(item => new RowContextMenuItem {Id = item.Kebaberize(), Name = item.Humanize()});
            return this;
        }

        public RowInfoBuilder ContextMenuItems(params RowContextMenuItem[] items)
        {
            Check.NotNull(items, nameof(items));

            _contextMenuItems = items.ToList();
            return this;
        }

        public RowInfo Build()
        {
            return new RowInfo {Selection = _selectionType, ShowContextMenu = _showContextMenu, ContextMenuItems = _contextMenuItems};
        }
    }
}
