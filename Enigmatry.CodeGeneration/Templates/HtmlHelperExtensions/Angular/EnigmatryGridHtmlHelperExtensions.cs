using System;
using System.Collections.Generic;
using System.Linq;
using Enigmatry.CodeGeneration.Configuration;
using Enigmatry.CodeGeneration.Configuration.Formatters;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Enigmatry.CodeGeneration.Configuration.List.Model;
using Humanizer;

namespace Enigmatry.CodeGeneration.Templates.HtmlHelperExtensions.Angular
{
    public static class EnigmatryGridHtmlHelperExtensions
    {
        public static IHtmlContent CreateColumnDefs(this IHtmlHelper html, IEnumerable<ColumnDefinitionModel> columns)
        {
            var columnDefs = columns.Select(CreateColumnDef);
            return html.Raw($"[\n{String.Join(",\n", columnDefs)}\n]");
        }

        private static string CreateColumnDef(ColumnDefinitionModel column)
        {
            return JsObject(
                JsProperty("field", column.Property.Camelize()),
                JsProperty("header", column.HeaderName),
                JsProperty("hide", !column.IsVisible),
                JsProperty("sortable", column.IsSortable),
                JsProperty("type", ColumnDefType(column))
            );
        }

        private static string JsObject(params string[] properties)
        {
            return $"{{ {String.Join(", ", properties.Where(property => property.HasContent()))} }}";
        }

        private static string JsProperty(string name, string value)
        {
            return $"{name}: \'{value}\'";
        }

        private static string JsProperty(string name, bool value)
        {
            return $"{name}: {value.ToString().ToLower()}";
        }

        private static string ColumnDefType(ColumnDefinitionModel column)
        {
            return column.Formatter switch
            {
                { } when column.Formatter is DatePropertyFormatter => "date",
                { } when column.Formatter is CurrencyPropertyFormatter => "currency",
                { } when column.Formatter is DecimalPropertyFormatter => "number",
                { } when column.Formatter is PercentPropertyFormatter => "percent",
                { } when column.Formatter is BooleanPropertyFormatter => "boolean",
                { } when column.Formatter is NoFormattingPropertyFormatter => String.Empty,
                _ => throw new NotImplementedException("Formatter type not supported")
            };
        }
    }
}
