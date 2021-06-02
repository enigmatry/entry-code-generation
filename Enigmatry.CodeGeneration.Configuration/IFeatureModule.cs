using System.Collections.Generic;
using Enigmatry.CodeGeneration.Configuration.Services;

namespace Enigmatry.CodeGeneration.Configuration
{
    public interface IFeatureModule
    {
        public string Name { get; }
        public IEnumerable<IComponentModel> Components { get; }
        public IEnumerable<ModuleImport> Imports { get; }
        IEnumerable<IServiceModel> Services { get; }
    }
}
