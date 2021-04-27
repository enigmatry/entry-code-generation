using System;

namespace Enigmatry.Blueprint.CodeGeneration.Configuration
{
    public class RoutingInfo
    {
        public string Path { get; set; }
        public bool IsRoutingEnabled { get; set; } = true;

        public RoutingInfo(string path)
        {
            Path = path;
        }

        public static RoutingInfo NoRouting()
        {
            return new RoutingInfo(String.Empty) {IsRoutingEnabled = false};
        }
    }
}
