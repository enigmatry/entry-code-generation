using System;
using Humanizer;

namespace Enigmatry.CodeGeneration.Configuration.Builder
{
    public class ComponentInfoBuilder : IBuilder<ComponentInfo>
    {
        protected string _componentName;
        protected string _featureName;
        protected readonly string _modelTypeName;

        public ComponentInfoBuilder(Type modelType)
        {
            _componentName = modelType.Name;
            _featureName = modelType.Name;
            _modelTypeName = modelType.GetDeclaringName();
        }

        public ComponentInfoBuilder HasName(string name)
        {
            _componentName = name.Pascalize();
            return this;
        }

        public ComponentInfoBuilder BelongsToFeature(string featureName)
        {
            _featureName = featureName.Pascalize();
            return this;
        }

        public ComponentInfo Build()
        {
            return new ComponentInfo {Name = _componentName, FeatureName = _featureName, ModelTypeName = _modelTypeName};
        }
    }
}
