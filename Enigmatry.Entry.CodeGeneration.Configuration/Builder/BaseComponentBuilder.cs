using System;

namespace Enigmatry.Entry.CodeGeneration.Configuration.Builder;

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

    public FeatureInfoBuilder Feature() => _componentInfoBuilder.Feature();

    public abstract T Build();
}
