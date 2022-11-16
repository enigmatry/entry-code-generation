using System;
using System.Collections.Generic;
using System.Linq;

namespace Enigmatry.Entry.CodeGeneration.Configuration
{
    public class FeatureInfo
    {
        public string Name { get; }
        public IEnumerable<ModuleImport> Imports { get; }

        public FeatureInfo(string name, IEnumerable<ModuleImport> imports)
        {
            Name = name;
            Imports = imports;
        }

        public static FeatureInfo None()
        {
            return new FeatureInfo(String.Empty, Enumerable.Empty<ModuleImport>());
        }
    }
}
