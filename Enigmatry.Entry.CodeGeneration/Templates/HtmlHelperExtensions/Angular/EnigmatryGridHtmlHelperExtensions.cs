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

    public static IHtmlContent AllCustomCellTemplateViewChildRefs(this IHtmlHelper html, IEnumerable<ColumnDefinition> columns, bool withSignals)
    {
        var customComponents = columns.Where(c => c.HasCustomCellComponent);
        var viewChildTemplateRefs = customComponents.Select(component => CustomCellTemplateViewChildRef(component, withSignals));
        var htmlContent = $"{string.Join("\r\n", viewChildTemplateRefs)}\r\n";

        return html.Raw(htmlContent);
    }

    public static IHtmlContent CreateColumnDefinitions(this IHtmlHelper html, IEnumerable<ColumnDefinition> columns, bool enableI18N)
    {
        var columnDefinitions = columns.Select(definition => CreateColumnDef(definition, enableI18N)).ToList();
        var htmlContent = columnDefinitions.Any() ? $"[\r\n{String.Join(",\r\n", columnDefinitions)}\r\n]" : "[]";

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
        var sortProperties = new KeyValuePair<string, string>[] { new("id", column.SortId ?? string.Empty) };

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
            JsProperty("sortProperties", JsObject(sortProperties), !column.SortId.HasContent(), true)
        );
    }

    private static string CustomCellTemplateViewChildRef(ColumnDefinition column, bool withSignals)
    {
        var templateRefId = CustomCellTemplateRefId(column);
        return withSignals ?
            $"protected readonly {templateRefId} = viewChild<TemplateRef<unknown>>('{templateRefId}');" :
            $"@ViewChild('{templateRefId}', {{ static: true }}) {templateRefId}: TemplateRef<unknown>;";
    }

    private static string CustomCellTemplateRefId(ColumnDefinition column)
    {
        var templateRefId = column.Property.Replace(".", "_").Camelize();
        return $"{templateRefId}Template";
    }

    private static string JsObject(params string?[] properties) => $"{{ {String.Join(", ", properties.Where(property => property.HasContent()))} }}";

    private static string JsObject(IEnumerable<KeyValuePair<string, string>> properties) => $"{{ {string.Join(", ", properties.Select(keyValue => JsProperty(keyValue.Key.Camelize(), keyValue.Value)))} }}";

    private static string? JsProperty(string name, string value, bool skip = false, bool asObject = false)
    {
        var jsProperty = !asObject ? $"{name}: \'{value}\'" : $"{name}: {value}";
        return !skip ? jsProperty : null;
    }

    private static string? JsProperty(string name, bool value, bool skip = false) => !skip ? $"{name}: {value.ToString().ToLower()}" : null;

    #endregion
}
