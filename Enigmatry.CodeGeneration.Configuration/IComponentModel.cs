namespace Enigmatry.CodeGeneration.Configuration
{
    public interface IComponentModel
    {
        ComponentInfo ComponentInfo { get; }
        RoutingInfo RoutingInfo { get; }
        ApiClientInfo ApiClientInfo { get; }
        FeatureInfo FeatureInfo { get; }
    }
}
