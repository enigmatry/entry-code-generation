using System.Collections.Generic;
using System.Linq;
using Enigmatry.CodeGeneration.Configuration.List.Model;

namespace Enigmatry.CodeGeneration.Configuration.List
{
    public class ListComponentModel : ComponentModel
    {
        public IList<ColumnPropertyModel> Columns { get; set; }

        public ListComponentModel(ComponentInfo componentInfo, RoutingInfo routingInfo, ApiClientInfo apiClientInfo,
            IList<ColumnPropertyModel> columns)
        : base(componentInfo, routingInfo, apiClientInfo)
        {
            Columns = columns;
        }

        public IEnumerable<ColumnPropertyModel> VisibleColumns => Columns.Where(column => column.IsVisible);
    }
}
