using System;
using Humanizer;

namespace Enigmatry.Entry.CodeGeneration.Configuration.Builder;

/// <summary>
/// ComponentInfoBuilder is a class that allows you to fluently configure various settings for a component, such as
/// the component name, the feature it belongs to, translation IDs, and the order of columns or form controls.
/// </summary>
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

    /// <summary>
    /// Set the name of the generated Angular component.
    /// </summary>
    /// <param name="name">The name of the Angular component.</param>
    /// <returns>The ComponentInfoBuilder instance for method chaining.</returns>
    public ComponentInfoBuilder HasName(string name)
    {
        _componentName = name.Pascalize();
        return this;
    }

    /// <summary>
    /// Set the feature name to which the component belongs.
    /// </summary>
    /// Components belonging to the same feature will be added to the same Angular module.
    /// <remarks>
    /// </remarks>
    /// <param name="featureName">The name of the feature.</param>
    /// <returns>The ComponentInfoBuilder instance for method chaining.</returns>
    public ComponentInfoBuilder BelongsToFeature(string featureName)
    {
        _featureInfoBuilder.WithName(featureName.Pascalize());
        return this;
    }

    /// <summary>
    /// Set the translation ID prefix for table columns or form controls, used when the code generator is configured to use Angular i18n.
    /// </summary>
    /// <param name="translationId">The translation ID prefix.</param>
    /// <returns>The ComponentInfoBuilder instance for method chaining.</returns>
    public ComponentInfoBuilder WithTranslationId(string translationId)
    {
        _translationId = translationId;
        return this;
    }

    /// <summary>
    /// Include or exclude unconfigured properties when generating table columns or form controls.
    /// </summary>
    /// <param name="value">Set to true to include unconfigured properties (default), false to exclude them.</param>
    /// <returns>The ComponentInfoBuilder instance for method chaining.</returns>
    public ComponentInfoBuilder IncludeUnconfiguredProperties(bool value = true)
    {
        _includeUnconfiguredProperties = value;
        return this;
    }

    /// <summary>
    /// Set the order of columns or form controls, based on either the properties order in the class (Model) or their definitions order in the Configure method (Configuration).
    /// </summary>
    /// <param name="orderByType">The type of order, either Model or Configuration.</param>
    /// <returns>The ComponentInfoBuilder instance for method chaining.</returns>
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
