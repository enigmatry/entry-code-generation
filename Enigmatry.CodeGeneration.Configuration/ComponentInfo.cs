using System;
using Humanizer;

namespace Enigmatry.CodeGeneration.Configuration
{
    public class ComponentInfo
    {
        public string Name { get; set; }
        public string ModelType { get; set; }
        public bool IncludeUnconfiguredProperties { get; }
        public OrderByType OrderByType { get; }
        public RoutingInfo Routing { get; set; }
        public ApiClientInfo ApiClient { get; set; }
        public FeatureInfo Feature { get; set; }
        public string TranslationId { get; }
        public string DefaultTranslationId => $"{Feature.Name.Kebaberize()}.{Name.Kebaberize()}";

        public ComponentInfo(string name, string modelType, bool includeUnconfiguredProperties, OrderByType orderByType,
            RoutingInfo routing, ApiClientInfo apiClient, FeatureInfo feature, string? translationId = null)
        {
            Name = name;
            ModelType = modelType;
            IncludeUnconfiguredProperties = includeUnconfiguredProperties;
            OrderByType = orderByType;
            Routing = routing;
            ApiClient = apiClient;
            Feature = feature;
            TranslationId = translationId ?? DefaultTranslationId;
        }

        public ComponentInfo(string name)
        : this(name, String.Empty, true, OrderByType.Model, RoutingInfo.NoRouting(), ApiClientInfo.NoApiClient(), FeatureInfo.None())
        { }
    }
}
