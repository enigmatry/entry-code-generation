using System.Collections.Generic;

namespace Enigmatry.CodeGeneration.Configuration.Builder
{
    public class FeatureInfoBuilder : IBuilder<FeatureInfo>
    {
        private readonly IList<ModuleImport> _moduleImports = new List<ModuleImport>();

        public FeatureInfoBuilder RequiresModules(string name, string path)
        {
            Check.NotEmpty(name, nameof(name));
            Check.NotEmpty(path, nameof(path));

            _moduleImports.Add(new ModuleImport {Name = name, Path = path});
            return this;
        }

        public FeatureInfo Build()
        {
            return new FeatureInfo {Imports = _moduleImports};
        }
    }
}
