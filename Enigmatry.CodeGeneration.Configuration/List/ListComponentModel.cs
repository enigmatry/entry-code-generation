using System.Collections.Generic;
using System.Linq;
using Enigmatry.CodeGeneration.Configuration.List.Model;

namespace Enigmatry.CodeGeneration.Configuration.List
{
    public class ListComponentModel : IComponentModel
    {
        public ComponentInfo ComponentInfo { get; }
        public IList<ColumnDefinitionModel> Columns { get; }
        public bool ShowPaginator { get; set; } = true;
        public bool ShowFirstLastPageButtons { get; set; }
        public IEnumerable<int> PageSizeOptions { get; set; } = new[] {10, 50, 100};
        public bool HidePageSize { get; set; }
        public bool EnableSingleSelection { get; set; }
        public bool EnableMultiSelection { get; set; }
        public bool IsRowSelectable => EnableSingleSelection || EnableMultiSelection;

        public ListComponentModel(ComponentInfo componentInfo, IEnumerable<ColumnDefinitionModel> columns)
        {
            ComponentInfo = componentInfo;
            Columns = columns.ToList();
        }

        public IEnumerable<ColumnDefinitionModel> VisibleColumns => Columns.Where(column => column.IsVisible);
    }
}
