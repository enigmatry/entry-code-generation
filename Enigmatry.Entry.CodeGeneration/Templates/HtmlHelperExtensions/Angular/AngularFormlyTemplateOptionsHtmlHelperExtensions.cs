using System;
using System.Collections.Generic;
using System.Linq;
using Enigmatry.Entry.CodeGeneration.Configuration.Form.Controls;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Enigmatry.Entry.CodeGeneration.Templates.HtmlHelperExtensions.Angular;

public static class AngularFormlyTemplateOptionsHtmlHelperExtensions
{
    public static IHtmlContent AddAttributes(this IHtmlHelper html, FormControl control)
    {
        var attributes = String.Empty;

        if (control is InputControlBase { ShouldAutocomplete: { } } inputControl)
        {
            attributes += $"autocomplete: '{(inputControl.ShouldAutocomplete.Value ? "on" : "off")}'";
        }
        return html.Raw($"attributes: {{ {attributes} }},");
    }

    public static IHtmlContent AddMetadata(this IHtmlHelper html, IEnumerable<KeyValuePair<string, string>> metadata)
    {
        return metadata.Any()
            ? html.Raw($"metadata: {{ {String.Join(", ", metadata.Select(x => $"{x.Key}: '{x.Value}'"))} }},\r\n")
            : html.Raw("");
    }
}
