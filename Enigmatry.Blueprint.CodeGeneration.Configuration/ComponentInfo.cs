using System;

namespace Enigmatry.Blueprint.CodeGeneration.Configuration
{
    public class ComponentInfo
    {
        public string Name { get; set; } = String.Empty;
        public string FeatureName { get; set; } = String.Empty;
        public string ModelTypeName { get; set; } = String.Empty;
        public RoutingInfo Routing { get; set; } = RoutingInfo.NoRouting();
    }
}
