using System;
using Humanizer;

namespace Enigmatry.Entry.CodeGeneration.Configuration.Builder;

public class ComponentInfoBuilder : IBuilder<ComponentInfo>
{
    private string _componentName;
    private readonly string _modelType;
    private readonly FeatureInfoBuilder _featureInfoBuilder;
    private string? _translationId;
    private bool _includeUnconfiguredProperties = true;
    private OrderByType _orderByType = OrderByType.Model;

    public ComponentInfoBuilder(Type modelType)
    {
        _componentName = modelType.Name;
        _modelType = modelType.GetDeclaringName();
        _featureInfoBuilder = new FeatureInfoBuilder();
    }

    public ComponentInfoBuilder HasName(string name)
    {
        _componentName = name.Pascalize();
        return this;
    }

    public ComponentInfoBuilder BelongsToFeature(string featureName)
    {
        _featureInfoBuilder.WithName(featureName.Pascalize());
        return this;
    }

    public ComponentInfoBuilder WithTranslationId(string translationId)
    {
        _translationId = translationId;
        return this;
    }

    public ComponentInfoBuilder IncludeUnconfiguredProperties(bool value = true)
    {
        _includeUnconfiguredProperties = value;
        return this;
    }

    public ComponentInfoBuilder OrderBy(OrderByType orderByType)
    {
        _orderByType = orderByType;
        return this;
    }

    public FeatureInfoBuilder Feature() => _featureInfoBuilder;

    public ComponentInfo Build()
    {
        return new ComponentInfo(_componentName, _modelType, _includeUnconfiguredProperties, _orderByType, _featureInfoBuilder.Build(), _translationId);
    }
}
