using System;

namespace Enigmatry.Entry.CodeGeneration.Configuration.Builder;

public class RoutingInfoBuilder : IBuilder<RoutingInfo>
{
    private string _route = String.Empty;

    public RoutingInfoBuilder WithRoute(string route)
    {
        _route = route;
        return this;
    }

    public RoutingInfoBuilder WithEmptyRoute()
    {
        return WithRoute(String.Empty);
    }

    public RoutingInfoBuilder WithIdRoute()
    {
        return WithRoute(":id");
    }

    public RoutingInfo Build()
    {
        return new RoutingInfo(_route);
    }
}