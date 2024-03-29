﻿using Enigmatry.Entry.CodeGeneration.Configuration;
using Humanizer;

namespace Enigmatry.Entry.CodeGeneration.Angular;

public static class ComponentModelExtensions
{
    public static string AngularComponentName(this IComponentModel component)
    {
        return $"{component.ComponentInfo.Name.Pascalize()}GeneratedComponent";
    }

    public static string AngularComponentDirectory(this IComponentModel component)
    {
        return $"{component.ComponentInfo.Name.Kebaberize()}";
    }

    public static string AngularComponentFileName(this IComponentModel component)
    {
        return $"{component.ComponentInfo.Name.Kebaberize()}-generated.component";
    }

    public static string AngularComponentSelector(this IComponentModel component, string prefix)
    {
        return $"{prefix}-{component.ComponentInfo.Name.Kebaberize()}".ToLower();
    }
}
