using System;
using Enigmatry.Entry.CodeGeneration.Configuration;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Enigmatry.Entry.CodeGeneration.Templates.HtmlHelperExtensions.Angular;

public static class AngularLocalization
{
    public static IHtmlContent Localize(this IHtmlHelper html, I18NString? text, bool enableI18N)
    {
        if (enableI18N && text != null && text.Key.HasContent() && text.Value.HasContent())
        {
            return html.Raw(Localize(text.Key, text.Value));
        }
        return html.Raw($"`{text?.Value ?? String.Empty}`");
    }

    public static string Localize(string key, string value)
    {
        return $"$localize `:@@{key}:{value}`";
    }
}
