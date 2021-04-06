using Enigmatry.Blueprint.CodeGeneration.Configuration.Services;
using System.Collections.Generic;

namespace Enigmatry.Blueprint.CodeGeneration.Configuration
{
    public interface IFeatureModule : IWithServices
    {
        public string Name { get; }
        public IEnumerable<IComponentModel> Components { get; }
    }
}
