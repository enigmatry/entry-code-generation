﻿using System.Collections.Generic;

namespace Enigmatry.CodeGeneration.Configuration
{
    public interface IFeatureModule
    {
        public string Name { get; }
        public IEnumerable<IComponentModel> Components { get; }
        public IEnumerable<ModuleImport> Imports { get; }
    }
}
