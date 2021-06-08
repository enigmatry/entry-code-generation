using System.Collections.Generic;
using System.Linq;
using Enigmatry.CodeGeneration.Configuration.Services;

namespace Enigmatry.CodeGeneration.Configuration
{
    public class FeatureModule : IFeatureModule
    {
        public string Name { get; set; }
        public IEnumerable<IComponentModel> Components { get; set; }
        public IEnumerable<ModuleImport> Imports { get; set; }
        public IEnumerable<IServiceModel> Services { get; set; }

        public FeatureModule(string name, IEnumerable<IComponentModel> components)
        {
            Name = name;
            Components = components.ToList();

            Imports = Components.SelectMany(c => c.ComponentInfo.Feature.Imports).ToList();

            Services = Components
                .Select(component => component as IWithLookupService)
                .Select(component => component?.LookupService)
                .Where(service => service != null)!;
        }

        public FeatureModule(IGrouping<string, IComponentModel> components)
            : this(components.Key, components)
        {
        }
    }
}
