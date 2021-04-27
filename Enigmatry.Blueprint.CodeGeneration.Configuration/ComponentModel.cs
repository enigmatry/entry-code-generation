namespace Enigmatry.Blueprint.CodeGeneration.Configuration
{
    public abstract class ComponentModel : IComponentModel
    {
        public ComponentInfo ComponentInfo { get; set; }
        public RoutingInfo RoutingInfo { get; set; }
        public ApiClientInfo ApiClientInfo { get; set; }

        protected ComponentModel(ComponentInfo componentInfo)
            : this(componentInfo, RoutingInfo.NoRouting(), ApiClientInfo.NoApiClient())
        {
        }

        protected ComponentModel(
            ComponentInfo componentInfo,
            RoutingInfo routingInfo,
            ApiClientInfo apiClientInfo)
        {
            ComponentInfo = componentInfo;
            RoutingInfo = routingInfo;
            ApiClientInfo = apiClientInfo;
        }
    }
}
