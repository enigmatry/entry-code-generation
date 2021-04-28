﻿using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Enigmatry.Blueprint.CodeGeneration.Templates.HtmlHelperExtensions
{
    public static class DisclaimerHtmlHelperExtensions
    {
        public static IHtmlContent Disclaimer(this IHtmlHelper html)
        {
            return html.Raw(
@"// ------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
// ------------------------------------------------------------------------------;");
        }
    }
}
