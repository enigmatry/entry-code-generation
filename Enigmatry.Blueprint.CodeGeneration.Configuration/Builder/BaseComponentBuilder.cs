using System;

namespace Enigmatry.Blueprint.CodeGeneration.Configuration.Builder
{
    public abstract class BaseComponentBuilder<T> : IComponentBuilder<T> where T : IComponentModel
    {
        protected readonly Type _modelType;
        protected readonly ComponentInfoBuilder _componentInfoBuilder;

        protected BaseComponentBuilder(Type modelType)
        {
            _modelType = modelType;
            _componentInfoBuilder = new ComponentInfoBuilder(modelType);
        }

        public ComponentInfoBuilder Component() => _componentInfoBuilder;

        public abstract T Build();
    }
}
