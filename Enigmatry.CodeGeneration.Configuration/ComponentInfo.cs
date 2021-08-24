using System;
using Humanizer;

namespace Enigmatry.CodeGeneration.Configuration
{
    public class ComponentInfo
    {
        public string Name { get; set; }
        public string ModelType { get; set; }
        public RoutingInfo Routing { get; set; }
        public ApiClientInfo ApiClient { get; set; }
        public FeatureInfo Feature { get; set; }
        public string TranslationId { get; }
        public string DefaultTranslationId => $"{Feature.Name.Kebaberize()}.{Name.Kebaberize()}";

        public ComponentInfo(string name, string modelType, 
            RoutingInfo routing, ApiClientInfo apiClient, FeatureInfo feature, string? translationId = null)
        {
            Name = name;
            ModelType = modelType;
            Routing = routing;
            ApiClient = apiClient;
            Feature = feature;
            TranslationId = translationId ?? DefaultTranslationId;
        }

        public ComponentInfo(string name)
        : this(name, String.Empty, RoutingInfo.NoRouting(), ApiClientInfo.NoApiClient(), FeatureInfo.None())
        { }
    }
}
