﻿using Enigmatry.Blueprint.CodeGeneration.Configuration;
using Humanizer;

namespace Enigmatry.Blueprint.CodeGeneration.Angular
{
    public static class IComponentModelExtensions
    {
        public static string AngularComponentName(this IComponentModel component)
        {
            return $"{component.ComponentInfo.Name.Pascalize()}Component";
        }

        public static string AngularComponentDirectory(this IComponentModel component)
        {
            return $"{component.ComponentInfo.Name.Kebaberize()}";
        }

        public static string AngularComponentFileName(this IComponentModel component)
        {
            return $"{component.ComponentInfo.Name.Kebaberize()}.component";
        }

        public static string AngularComponentApiClient(this IComponentModel component)
        {
            return $"{component.ComponentInfo.FeatureName.Pascalize()}Client";
        }
    }
}
