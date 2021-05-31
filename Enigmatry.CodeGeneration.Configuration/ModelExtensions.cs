using System.Collections.Generic;
using System.Linq;

namespace Enigmatry.CodeGeneration.Configuration
{
    public static class ModelExtensions
    {
        public static IEnumerable<IFeatureModule> GroupByFeature(this IEnumerable<IComponentModel> components)
        {
            return components.GroupBy(component => component.ComponentInfo.FeatureName)
                .Select(grouping => new FeatureModule(grouping));
        }
    }
}
