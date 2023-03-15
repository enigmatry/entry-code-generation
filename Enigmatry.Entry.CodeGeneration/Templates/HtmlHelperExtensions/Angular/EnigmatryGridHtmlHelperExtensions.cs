using System;
using System.Collections.Generic;
using System.Linq;
using Enigmatry.Entry.CodeGeneration.Configuration;
using Enigmatry.Entry.CodeGeneration.Configuration.List.Model;
using Humanizer;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Enigmatry.Entry.CodeGeneration.Templates.HtmlHelperExtensions.Angular;

public static class EnigmatryGridHtmlHelperExtensions
{
    public static IHtmlContent CustomCellTemplateRefId(this IHtmlHelper html, ColumnDefinition column) 
        => html.Raw(CustomCellTemplateRefId(column));

    public static IHtmlContent CustomCellTemplateViewChildRef(this IHtmlHelper html, ColumnDefinition column) 
        => html.Raw(CustomCellTemplateViewChildRef(column));

    public static IHtmlContent AllCustomCellTemplateViewChildRefs(this IHtmlHelper html, IEnumerable<ColumnDefinition> columns)
    {
        var customComponents = columns.Where(c => c.HasCustomCellComponent);
        var viewChildTemplateRefs = customComponents.Select(CustomCellTemplateViewChildRef);
        var htmlContent = $"{String.Join("\r\n", viewChildTemplateRefs)}\r\n";

        return html.Raw(htmlContent);
    }

    public static IHtmlContent CreateColumnDefs(this IHtmlHelper html, IEnumerable<ColumnDefinition> columns, bool enableI18N)
    {
        var columnDefs = columns.Select(definition => CreateColumnDef(definition, enableI18N)).ToList();
        var htmlContent = columnDefs.Any() ? $"[\r\n{String.Join(",\r\n", columnDefs)}\r\n]" : "[]";

        return html.Raw(htmlContent);
    }

    public static IHtmlContent CreateContextMenuItems(this IHtmlHelper html, IEnumerable<RowContextMenuItem> items, bool enableI18N)
    {
        var contextMenuItems = items.Select(item => ContextMenuItemToJs(item, enableI18N)).ToList();
        var htmlContent = contextMenuItems.Any() ? $"[\r\n{String.Join(",\r\n", contextMenuItems)}\r\n]" : "[]";

        return html.Raw(htmlContent);
    }

    private static string ContextMenuItemToJs(RowContextMenuItem item, bool enableI18N)
    {
        var name = enableI18N ? AngularLocalization.Localize(item.TranslationId!, item.Name) : item.Name;

        return JsObject(
            JsProperty("id", item.Id),
            JsProperty("name", name, false, enableI18N),
            JsProperty("icon", item.Icon ?? String.Empty, !item.Icon.HasContent()));
    }

    #region [private]

    private static string CreateColumnDef(ColumnDefinition column, bool enableI18N)
    {
        var columnType = column.Formatter.JsFormatterName;
        var columnTypeParams = column.Formatter.ToJsObject();
        var propertyName = column.Property.Camelize();
        var cellTemplate = $"this.{CustomCellTemplateRefId(column)}";

        var header = enableI18N ? AngularLocalization.Localize(column.TranslationId, column.HeaderName) : column.HeaderName;
        var sortProps = new KeyValuePair<string, string>[] { new("id", column.SortId ?? String.Empty) };

        return JsObject(
            JsProperty("field", propertyName),
            JsProperty("header", header, !column.IsVisible, enableI18N),
            JsProperty("hide", !column.IsVisible),
            JsProperty("sortable", column.IsSortable),
            JsProperty("type", columnType, !columnType.HasContent()),
            JsProperty("typeParameter", columnTypeParams, !columnType.HasContent(), true),
            JsProperty("cellTemplate", cellTemplate, !column.HasCustomCellComponent, true),
            JsProperty("class", column.CustomCellCssClass ?? "", !column.HasCustomCellCssClass),
            JsProperty("customProperties", JsObject(column.CustomProperties), !column.CustomProperties.Any(), true),
            JsProperty("sortProp", JsObject(sortProps), !column.SortId.HasContent(), true)
        );
    }

    private static string CustomCellTemplateViewChildRef(ColumnDefinition column)
    {
        var templateRefId = CustomCellTemplateRefId(column);
        return $"@ViewChild('{templateRefId}', {{ static: true }}) {templateRefId}: TemplateRef<any>;";
    }

    private static string CustomCellTemplateRefId(ColumnDefinition column)
    {
        var templateRefId = column.Property.Replace(".", "_").Camelize();
        return $"{templateRefId}Tpl";
    }

    private static string JsObject(params string?[] properties)
    {
        return $"{{ {String.Join(", ", properties.Where(property => property.HasContent()))} }}";
    }

    private static string JsObject(IEnumerable<KeyValuePair<string, string>> properties)
    {
        return $"{{ {String.Join(", ", properties.Select(keyValue => JsProperty(keyValue.Key.Camelize(), keyValue.Value)))} }}";
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