using System.Collections.Generic;
using System.Linq;
using Enigmatry.CodeGeneration.Configuration.List.Model;

namespace Enigmatry.CodeGeneration.Configuration.List
{
    public class ListComponentModel : IComponentModel
    {
        public ComponentInfo ComponentInfo { get; }
        public IList<ColumnDefinitionModel> Columns { get; }

        public ListComponentModel(ComponentInfo componentInfo, IEnumerable<ColumnDefinitionModel> columns)
        {
            ComponentInfo = componentInfo;
            Columns = columns.ToList();
        }

        public IEnumerable<ColumnDefinitionModel> VisibleColumns => Columns.Where(column => column.IsVisible);
    }
}
