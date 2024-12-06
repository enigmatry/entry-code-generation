namespace Enigmatry.Entry.CodeGeneration.Configuration.List.Model;

public class RowInfo
{
    public RowSelectionType Selection { get; set; } = RowSelectionType.None;
    public bool ShowContextMenu { get; set; }
    public IEnumerable<RowContextMenuItem> ContextMenuItems { get; set; } = Enumerable.Empty<RowContextMenuItem>();
    public bool IsSelectable => Selection == RowSelectionType.Single || Selection == RowSelectionType.Multiple;
    public bool ShowSelectAllOption { get; set; } = true;
    public ComponentInfo ComponentInfo { get; set; } = null!;
}