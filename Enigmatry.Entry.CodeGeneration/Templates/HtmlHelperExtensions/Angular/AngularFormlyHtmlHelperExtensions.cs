using System.Globalization;
using Enigmatry.Entry.CodeGeneration.Configuration;
using Enigmatry.Entry.CodeGeneration.Configuration.Form.Controls;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Enigmatry.Entry.CodeGeneration.Templates.HtmlHelperExtensions.Angular;

public static class AngularFormlyHtmlHelperExtensions
{
    public static IHtmlContent FieldCssClass(this IHtmlHelper html, FormControl control) => html.Raw($"className: `{control.StackedClasses()}`,\r\n");

    public static IHtmlContent GroupCssClass(this IHtmlHelper html, FormControlGroup controlGroup)
    {
        var classNameValue = controlGroup.ClassNames.Values.Aggregate("entry-field-group",
            (current, className) => current + $" {className.ApplyOptionally()}");

        return html.Raw($"fieldGroupClassName: `{classNameValue}`,\r\n");
    }

    public static IHtmlContent DefaultValue(this IHtmlHelper html, FormControl control) =>
        control switch
        {
            DatepickerFormControl formControl => html.RenderDefaultValue(formControl.DefaultValue),
            InputControlBase formControl => html.RenderDefaultValue(formControl.DefaultValue),
            CheckboxFormControl formControl => html.RenderDefaultValue(formControl.DefaultValue),
            RadioGroupFormControl formControl => html.RenderDefaultValue(formControl.DefaultValue),
            SelectFormControl formControl => html.RenderDefaultValue(formControl.DefaultValue),
            _ => html.Raw("")
        };

    private static IHtmlContent RenderDefaultValue(this IHtmlHelper html, bool? defaultValue) =>
        defaultValue.HasValue ? html.Raw($"defaultValue: {(defaultValue.Value ? "true" : "false")},\r\n") : html.Raw("");

    private static IHtmlContent RenderDefaultValue(this IHtmlHelper html, DateTimeOffset? defaultValue) =>
        defaultValue.HasValue
            // O - ISO 8601
            ? html.Raw($"defaultValue: '{defaultValue.Value.ToString("O", CultureInfo.InvariantCulture)}',\r\n")
            : html.Raw("");

    private static IHtmlContent RenderDefaultValue(this IHtmlHelper html, string? defaultValue) =>
        defaultValue.HasContent() ? html.Raw($"defaultValue: '{defaultValue}',\r\n") : html.Raw("");
}
