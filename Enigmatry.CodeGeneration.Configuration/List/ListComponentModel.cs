using System.Collections.Generic;
using System.Linq;
using Enigmatry.CodeGeneration.Configuration.List.Model;

namespace Enigmatry.CodeGeneration.Configuration.List
{
    public class ListComponentModel : ComponentModel
    {
        public IList<ColumnDefinitionModel> Columns { get; set; }

        public ListComponentModel(
            ComponentInfo componentInfo,
            RoutingInfo routingInfo,
            ApiClientInfo apiClientInfo,
            FeatureInfo featureInfo,
            IList<ColumnDefinitionModel> columns)
        : base(componentInfo, routingInfo, apiClientInfo, featureInfo)
        {
            Columns = columns;
        }

        public IEnumerable<ColumnDefinitionModel> VisibleColumns => Columns.Where(column => column.IsVisible);
    }
}
