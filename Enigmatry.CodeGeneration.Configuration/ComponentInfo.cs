using System;

namespace Enigmatry.CodeGeneration.Configuration
{
    public class ComponentInfo
    {
        public string Name { get; set; }
        public string ModelType { get; set; }
        public RoutingInfo Routing { get; set; }
        public ApiClientInfo ApiClient { get; set; }
        public FeatureInfo Feature { get; set; }

        public ComponentInfo(string name, string modelType, 
            RoutingInfo routing, ApiClientInfo apiClient, FeatureInfo feature)
        {
            Name = name;
            ModelType = modelType;
            Routing = routing;
            ApiClient = apiClient;
            Feature = feature;
        }

        public ComponentInfo(string name)
        : this(name, String.Empty, RoutingInfo.NoRouting(), ApiClientInfo.NoApiClient(), FeatureInfo.None())
        {
        }
    }
}
