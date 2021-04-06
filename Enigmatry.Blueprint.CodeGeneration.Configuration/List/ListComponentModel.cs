using System.Collections.Generic;
using System.Linq;
using Enigmatry.Blueprint.CodeGeneration.Configuration.List.Model;

namespace Enigmatry.Blueprint.CodeGeneration.Configuration.List
{
    public class ListComponentModel : IComponentModel
    {
        public ComponentInfo ComponentInfo { get; set; }
        public IList<ColumnPropertyModel> Columns { get; set; }

        public ListComponentModel(ComponentInfo componentInfo, IList<ColumnPropertyModel> columns)
        {
            ComponentInfo = componentInfo;
            Columns = columns;
        }

        public IEnumerable<ColumnPropertyModel> VisibleColumns => Columns.Where(column => column.IsVisible);
    }
}
