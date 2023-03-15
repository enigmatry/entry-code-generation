using Enigmatry.Entry.CodeGeneration.Configuration.Form;
using Enigmatry.Entry.CodeGeneration.Configuration.List;
using Humanizer;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Enigmatry.Entry.CodeGeneration.Templates.HtmlHelperExtensions.Template;

public static class TemplateHtmlHelperExtensions
{
    public static IHtmlContent FormCssClass(this IHtmlHelper html, FormComponentModel form)
    {
        string classNameValue = $"entry-{form.ComponentInfo.Name.Kebaberize()}-form";
        return html.Raw($"class=\"{classNameValue} entry-form\"");
    }

    public static IHtmlContent ListCssClass(this IHtmlHelper html, ListComponentModel list)
    {
        string classNameValue = $"entry-{list.ComponentInfo.Name.Kebaberize()}-table";
        return html.Raw($"class=\"{classNameValue} entry-table\"");
    }
}