using System;

namespace Enigmatry.Entry.CodeGeneration.Configuration.Builder;

/// <summary>
/// Base class for component builder classes.
/// </summary>
/// <remarks>
/// </remarks>
/// <typeparam name="T">Inherited component underlying type implementing <see cref="IComponentModel"/> interface.</typeparam>
public abstract class BaseComponentBuilder<T> : IComponentBuilder<T> where T : IComponentModel
{
    protected readonly Type _modelType;
    protected readonly ComponentInfoBuilder _componentInfoBuilder;

    protected BaseComponentBuilder(Type modelType)
    {
        _modelType = modelType;
        _componentInfoBuilder = new ComponentInfoBuilder(modelType);
    }

    /// <summary>
    /// Configure component-level options, such as the component name, feature name, and translation ID.
    /// </summary>
    /// <returns>An instance of <see cref="ComponentInfoBuilder" /> to further configure the component.</returns>
    public ComponentInfoBuilder Component() => _componentInfoBuilder;

    public RoutingInfoBuilder Routing() => _componentInfoBuilder.Routing();

    public ApiClientInfoBuilder ApiClient() => _componentInfoBuilder.ApiClient();

    /// <summary>
    /// Configure feature-level options, such as the feature name.
    /// </summary>
    /// <returns>An instance of <see cref="FeatureInfoBuilder" /> to further configure the component's feature.</returns>
    public FeatureInfoBuilder Feature() => _componentInfoBuilder.Feature();

    public abstract T Build();
}
