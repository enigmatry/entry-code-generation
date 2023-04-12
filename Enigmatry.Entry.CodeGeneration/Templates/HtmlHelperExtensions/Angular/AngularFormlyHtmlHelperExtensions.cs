using Enigmatry.Entry.CodeGeneration.Configuration;
using Enigmatry.Entry.CodeGeneration.Configuration.Form.Controls;
using Humanizer;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Globalization;

namespace Enigmatry.Entry.CodeGeneration.Templates.HtmlHelperExtensions.Angular;

public static class AngularFormlyHtmlHelperExtensions
{
    public static IHtmlContent FieldCssClass(this IHtmlHelper html, FormControl control)
    {
        string classNameValue = $"entry-{control.PropertyName.Kebaberize()}-field entry-{control.FormlyType.Kebaberize()}";

        foreach (var className in control.ClassNames.Values)
        {
            classNameValue += $" {ApplyOptionally(className)}";
        }

        return html.Raw($"className: `{classNameValue}`,\r\n");
    }

    public static IHtmlContent GroupCssClass(this IHtmlHelper html, FormControlGroup controlGroup)
    {
        string classNameValue = "entry-field-group";

        foreach (var className in controlGroup.ClassNames.Values)
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
        switch (control.FormlyType)
        {
            case FormlyTypes.DatePicker:
                return html.RenderFormlyDefaultValue(((DatepickerFormControl)control).DefaultValue);
            case FormlyTypes.Input:
                return html.RenderFormlyDefaultValue(((InputControlBase)control).DefaultValue);
            case FormlyTypes.TextArea:
                return html.RenderFormlyDefaultValue(((TextareaFormControl)control).DefaultValue);
            case FormlyTypes.CheckBox:
                return html.RenderFormlyDefaultValue(((CheckboxFormControl)control).DefaultValue);
            case FormlyTypes.Radio:
                return html.RenderFormlyDefaultValue(((RadioGroupFormControl)control).DefaultValue);
            case FormlyTypes.Select:
                return control.GetType() == typeof(SelectFormControl)
                    ? html.RenderFormlyDefaultValue(((SelectFormControl)control).DefaultValue)
                    : html.Raw("");
            default:
                return html.Raw("");
        }
    }
    private static IHtmlContent RenderFormlyDefaultValue(this IHtmlHelper html, bool? defaultValue) =>
        defaultValue.HasValue ? html.Raw($"defaultValue: {(defaultValue.Value ? "true" : "false")},\r\n") : html.Raw("");

    private static IHtmlContent RenderFormlyDefaultValue(this IHtmlHelper html, DateTimeOffset? defaultValue) =>
        defaultValue.HasValue
            // O - ISO 8601 = 2018-04-24T06:30:00.0000000
            ? html.Raw($"defaultValue: '{defaultValue.Value.ToString("O", CultureInfo.InvariantCulture)}',\r\n")
            : html.Raw("");

    private static IHtmlContent RenderFormlyDefaultValue(this IHtmlHelper html, string? defaultValue) =>
        defaultValue.HasContent() ? html.Raw($"defaultValue: '{defaultValue}',\r\n") : html.Raw("");
}
