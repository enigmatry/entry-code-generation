using System.Collections.Generic;
using System.Linq;
using Enigmatry.Entry.CodeGeneration.Configuration.List.Model;

namespace Enigmatry.Entry.CodeGeneration.Configuration.List;

public class ListComponentModel : IComponentModel
{
    public ComponentInfo ComponentInfo { get; }
    public IList<ColumnDefinition> Columns { get; }
    public PaginationInfo Pagination { get; set; } = new PaginationInfo();
    public RowInfo Row { get; set; } = new RowInfo();

    public ListComponentModel(ComponentInfo componentInfo, IEnumerable<ColumnDefinition> columns)
    {
        ComponentInfo = componentInfo;
        Columns = columns.ToList();
    }

    public IEnumerable<ColumnDefinition> VisibleColumns => Columns.Where(column => column.IsVisible);

    public bool HasColumns => Columns.Any();
}