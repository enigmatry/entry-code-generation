using System;
using System.Collections.Generic;
using System.Linq;
using Enigmatry.CodeGeneration.Configuration;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Enigmatry.CodeGeneration.Configuration.List.Model;
using Humanizer;

namespace Enigmatry.CodeGeneration.Templates.HtmlHelperExtensions.Angular
{
    public static class EnigmatryGridHtmlHelperExtensions
    {
        public static IHtmlContent CustomCellTemplateRefId(this IHtmlHelper html, ColumnDefinitionModel column) 
            => html.Raw(CustomCellTemplateRefId(column));

        public static IHtmlContent CustomCellTemplateViewChildRef(this IHtmlHelper html, ColumnDefinitionModel column) 
            => html.Raw(CustomCellTemplateViewChildRef(column));

        public static IHtmlContent AllCustomCellTemplateViewChildRefs(this IHtmlHelper html, IEnumerable<ColumnDefinitionModel> columns)
        {
            var customComponents = columns.Where(c => c.HasCustomCellComponent);
            var viewChildTemplateRefs = customComponents.Select(c => CustomCellTemplateViewChildRef(c));
            var htmlContent = String.Join("\n", viewChildTemplateRefs);

            return html.Raw(htmlContent);
        }

        public static IHtmlContent CreateColumnDefs(this IHtmlHelper html, IEnumerable<ColumnDefinitionModel> columns)
        {
            var columnDefinitions = columns.Select(CreateColumnDef);
            var htmlContent = $"[\n{String.Join(",\n", columnDefinitions)}\n]";

            return html.Raw(htmlContent);
        }

        #region [private]

        private static string CreateColumnDef(ColumnDefinitionModel column)
        {
            var columnType = column.Formatter.JsFormatterName;
            var columnTypeParams = column.Formatter.ToJsObject();
            var cellTemplate = $"this.{CustomCellTemplateRefId(column)}";

            return JsObject(
                JsProperty("field", column.Property.Camelize()),
                JsProperty("header", column.HeaderName),
                JsProperty("hide", !column.IsVisible),
                JsProperty("sortable", column.IsSortable),
                JsProperty("type", columnType, !columnType.HasContent()),
                JsProperty("typeParameter", columnTypeParams, !columnType.HasContent(), true),
                JsProperty("cellTemplate", cellTemplate, !column.HasCustomCellComponent, true)
            );
        }

        private static string CustomCellTemplateViewChildRef(ColumnDefinitionModel column)
        {
            var templateRefId = CustomCellTemplateRefId(column);
            return $"@ViewChild('{templateRefId}', {{ static: true }}) {templateRefId}: TemplateRef<any>;";
        }

        private static string CustomCellTemplateRefId(ColumnDefinitionModel column)
        {
            var templateRefId = column.Property.Replace(".", "_").Camelize();
            return $"{templateRefId}Tpl";
        }

        private static string JsObject(params string?[] properties)
        {
            return $"{{ {String.Join(", ", properties.Where(property => property.HasContent()))} }}";
        }

        private static string? JsProperty(string name, string value, bool skip = false, bool asObject = false)
        {
            var jsProperty = !asObject ? $"{name}: \'{value}\'" : $"{name}: {value}";
            return !skip ? jsProperty : null;
        }

        private static string? JsProperty(string name, bool value, bool skip = false)
        {
            return !skip ? $"{name}: {value.ToString().ToLower()}" : null;
        }

        #endregion
    }
}
