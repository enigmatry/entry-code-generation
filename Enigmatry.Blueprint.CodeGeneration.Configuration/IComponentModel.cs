namespace Enigmatry.Blueprint.CodeGeneration.Configuration
{
    public interface IComponentModel
    {
        ComponentInfo ComponentInfo { get; }
        RoutingInfo RoutingInfo { get; }
        ApiClientInfo ApiClientInfo { get; }
    }
}
