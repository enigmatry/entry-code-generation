using System.Globalization;
using Enigmatry.Entry.CodeGeneration.Configuration;
using Enigmatry.Entry.CodeGeneration.Configuration.Form.Controls;
using Humanizer;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Enigmatry.Entry.CodeGeneration.Templates.HtmlHelperExtensions.Angular;

public static class AngularFormlyHtmlHelperExtensions
{
    public static IHtmlContent FieldCssClass(this IHtmlHelper html, FormControl control)
    {
        var classNameValue = $"entry-{control.PropertyName.Kebaberize()}-field entry-{control.FormlyType.Kebaberize()}";

        foreach (OptionallyAppliedValue<string> className in control.ClassNames.Values)
        {
            classNameValue += $" {ApplyOptionally(className)}";
        }

        return html.Raw($"className: `{classNameValue}`,\r\n");
    }

    public static IHtmlContent GroupCssClass(this IHtmlHelper html, FormControlGroup controlGroup)
    {
        var classNameValue = "entry-field-group";

        foreach (OptionallyAppliedValue<string> className in controlGroup.ClassNames.Values)
        {
            classNameValue += $" {ApplyOptionally(className)}";
        }

        return html.Raw($"fieldGroupClassName: `{classNameValue}`,\r\n");
    }

    private static string ApplyOptionally(OptionallyAppliedValue<string> className)
    {
        return className.When switch
        {
            ApplyWhen.FormIsReadonly => $"${{this.applyOptionally('{className}', this.isReadonly)}}",
            ApplyWhen.FormIsNotReadonly => $"${{this.applyOptionally('{className}', !this.isReadonly)}}",
            ApplyWhen.Always => $"{className}",
            _ => $"{className}"
        };
    }

    public static IHtmlContent DefaultValue(this IHtmlHelper html, FormControl control)
    {
        return control switch
        {
            DatepickerFormControl formControl => html.RenderDefaultValue(formControl.DefaultValue),
            InputControlBase formControl => html.RenderDefaultValue(formControl.DefaultValue),
            CheckboxFormControl formControl => html.RenderDefaultValue(formControl.DefaultValue),
            RadioGroupFormControl formControl => html.RenderDefaultValue(formControl.DefaultValue),
            SelectFormControl formControl => html.RenderDefaultValue(formControl.DefaultValue),
            _ => html.Raw("")
        };
    }

    private static IHtmlContent RenderDefaultValue(this IHtmlHelper html, bool? defaultValue)
    {
        return defaultValue.HasValue ? html.Raw($"defaultValue: {(defaultValue.Value ? "true" : "false")},\r\n") : html.Raw("");
    }

    private static IHtmlContent RenderDefaultValue(this IHtmlHelper html, DateTimeOffset? defaultValue)
    {
        return defaultValue.HasValue
            // O - ISO 8601
            ? html.Raw($"defaultValue: '{defaultValue.Value.ToString("O", CultureInfo.InvariantCulture)}',\r\n")
            : html.Raw("");
    }

    private static IHtmlContent RenderDefaultValue(this IHtmlHelper html, string? defaultValue)
    {
        return defaultValue.HasContent() ? html.Raw($"defaultValue: '{defaultValue}',\r\n") : html.Raw("");
    }
}
