namespace Enigmatry.CodeGeneration.Configuration
{
    public abstract class ComponentModel : IComponentModel
    {
        public ComponentInfo ComponentInfo { get; set; }
        public RoutingInfo RoutingInfo { get; set; }
        public ApiClientInfo ApiClientInfo { get; set; }
        public FeatureInfo FeatureInfo { get; set; }

        protected ComponentModel(ComponentInfo componentInfo)
            : this(componentInfo, RoutingInfo.NoRouting(), ApiClientInfo.NoApiClient(), new FeatureInfo())
        {
        }

        protected ComponentModel(
            ComponentInfo componentInfo,
            RoutingInfo routingInfo,
            ApiClientInfo apiClientInfo,
            FeatureInfo featureInfo)
        {
            ComponentInfo = componentInfo;
            RoutingInfo = routingInfo;
            ApiClientInfo = apiClientInfo;
            FeatureInfo = featureInfo;
        }
    }
}
