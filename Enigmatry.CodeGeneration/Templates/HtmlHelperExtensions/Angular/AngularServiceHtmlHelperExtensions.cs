using System;
using Enigmatry.CodeGeneration.Configuration.Services;
using Enigmatry.CodeGeneration.Templates.HtmlHelperExtensions.TypeScript;
using Humanizer;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Enigmatry.CodeGeneration.Templates.HtmlHelperExtensions.Angular
{
    public static class AngularServiceHtmlHelperExtensions
    {
        public static IHtmlContent LookupServiceImportStatement(this IHtmlHelper html, LookupServiceModel? service)
        {
            if (service == null)
            {
                return html.Raw("");
            }
            return html.ImportStatement($"{service.Name}LookupService", $"../services/{service.Name.Kebaberize()}-lookup.service");
        }

        public static IHtmlContent LookupsServiceDependencyInjection(this IHtmlHelper html, LookupServiceModel service)
        {
            var dependencyInjection = $"public lookupService: {service.Name}LookupService";
            return html.Raw(dependencyInjection);
        }
    }
}
