using System.Collections.Generic;
using System.Linq;

namespace Enigmatry.CodeGeneration.Configuration
{
    public class FeatureInfo
    {
        public IEnumerable<ModuleImport> Imports { get; set; } = Enumerable.Empty<ModuleImport>();
    }
}
