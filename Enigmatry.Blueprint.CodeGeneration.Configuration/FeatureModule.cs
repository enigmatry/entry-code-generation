using Enigmatry.Blueprint.CodeGeneration.Configuration.Services;
using System.Collections.Generic;
using System.Linq;

namespace Enigmatry.Blueprint.CodeGeneration.Configuration
{
    public class FeatureModule : IFeatureModule
    {
        public string Name { get; set; }
        public IEnumerable<IComponentModel> Components { get; set; }
        public IEnumerable<IServiceModel> Services { get; set; }

        public FeatureModule(string name, IEnumerable<IComponentModel> components)
        {
            Name = name;
            Components = components.ToList();
            Services = components
                .Where(component => component is IWithLookupService)
                .Select(component => ((IWithLookupService)component).LookupService)
                .Where(service => service != null);
        }

        public FeatureModule(IGrouping<string, IComponentModel> components) : this(components.Key, components) { }
            
    }
}
