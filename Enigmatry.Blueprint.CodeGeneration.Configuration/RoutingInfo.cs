using System;

namespace Enigmatry.Blueprint.CodeGeneration.Configuration
{
    public class RoutingInfo
    {
        public bool IsRoutingEnabled { get; set; }
        public string Path { get; set; }

        public RoutingInfo(string path, bool isRoutingEnabled = true)
        {
            IsRoutingEnabled = isRoutingEnabled;
            Path = path;
        }

        public static RoutingInfo NoRouting()
        {
            return new RoutingInfo(String.Empty, false);
        }

        public static RoutingInfo WithEmptyRoute()
        {
            return new RoutingInfo(String.Empty);
        }

        public static RoutingInfo WithIdRoute()
        {
            return new RoutingInfo(":id");
        }
    }
}
