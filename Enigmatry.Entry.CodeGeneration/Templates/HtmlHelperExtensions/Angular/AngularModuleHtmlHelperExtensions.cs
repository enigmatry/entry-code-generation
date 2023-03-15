using System.Text;
using Enigmatry.Entry.CodeGeneration.Angular;
using Enigmatry.Entry.CodeGeneration.Configuration;
using Enigmatry.Entry.CodeGeneration.Templates.HtmlHelperExtensions.TypeScript;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

// ReSharper disable once CheckNamespace
namespace Enigmatry.Entry.CodeGeneration.Templates.HtmlHelperExtensions;

public static class AngularModuleHtmlHelperExtensions
{
    public static IHtmlContent ImportComponentsFrom(this IHtmlHelper html, IFeatureModule module)
    {
        var sb = new StringBuilder();

        foreach (var component in module.Components)
        {
            var fromPath = $"./{component.AngularComponentDirectory()}/{component.AngularComponentFileName()}";
            var angularComponentName = component.AngularComponentName();
            sb.AppendLine(ImportHtmlHelperExtensions.CreateImportStatement(angularComponentName, fromPath));
        }

        return html.Raw(sb.ToString());
    }
}