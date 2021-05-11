﻿using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

// ReSharper disable PossibleMultipleEnumeration
namespace Enigmatry.CodeGeneration.Templates.HtmlHelperExtensions.TypeScript
{
    public static class ImportHtmlHelperExtensions
    {
        public static IHtmlContent ImportStatement(this IHtmlHelper html, string imports, string fromPath)
        {
            return html.Raw(CreateImportStatement(imports, fromPath));
        }

        public static IHtmlContent ImportStatement(this IHtmlHelper html, IEnumerable<string> imports, string fromPath)
        {
            return html.Raw(CreateImportStatement(imports, fromPath));
        }

        public static string CreateImportStatement(IEnumerable<string> imports, string fromPath)
        {
            if (imports.IsNullOrEmpty() || fromPath.IsNullOrEmpty())
                return String.Empty;

            return $"import {{ {String.Join(", ", imports)} }} from '{fromPath}';";
        }

        public static string CreateImportStatement(string imports, string fromPath)
        {
            if (imports.IsNullOrEmpty() || fromPath.IsNullOrEmpty())
                return String.Empty;

            return $"import {{ {imports} }} from '{fromPath}';";
        }
    }
}