using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Enigmatry.Entry.CodeGeneration.Templates.HtmlHelperExtensions.Angular;

public static class AngularFormlyTemplateOptionsHtmlHelperExtensions
{
    public static IHtmlContent AddMetadata(this IHtmlHelper html, IEnumerable<KeyValuePair<string, string>> metadata) =>
        metadata.Any()
            ? html.Raw($"metadata: {{ {String.Join(", ", metadata.Select(x => $"{x.Key}: '{x.Value}'"))} }},\r\n")
            : html.Raw("");
}