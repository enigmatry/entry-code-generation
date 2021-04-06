using Enigmatry.Blueprint.CodeGeneration.Configuration.Services;
using Enigmatry.Blueprint.CodeGeneration.Templates.HtmlHelperExtensions.TypeScript;
using Humanizer;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;

namespace Enigmatry.Blueprint.CodeGeneration.Templates.HtmlHelperExtensions.Angular
{
    public static class AngularServiceHtmlHelperExtensions
    {
        public static IHtmlContent LookupServiceImportStatement(this IHtmlHelper html, LookupServiceModel service)
        {
            return service == null
                ? html.Raw(String.Empty)
                : html.ImportStatement($"{service.Name}LookupService", $"../services/{service.Name.Kebaberize()}-lookup.service");
        }

        public static IHtmlContent LookupsServiceDependencyInjection(this IHtmlHelper html, LookupServiceModel service)
        {
            var dependencyInjection = service == null
                ? String.Empty
                : $",{Environment.NewLine}\t\tpublic lookupService: {service.Name}LookupService";
            return html.Raw(dependencyInjection);
        }
    }
}
