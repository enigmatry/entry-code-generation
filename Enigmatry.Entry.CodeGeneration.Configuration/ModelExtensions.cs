using System.Collections.Generic;
using System.Linq;

namespace Enigmatry.Entry.CodeGeneration.Configuration
{
    public static class ModelExtensions
    {
        public static IEnumerable<IFeatureModule> GroupByFeature(this IEnumerable<IComponentModel> components)
        {
            return components.GroupBy(component => component.ComponentInfo.Feature.Name)
                .Select(grouping => new FeatureModule(grouping));
        }
    }
}
