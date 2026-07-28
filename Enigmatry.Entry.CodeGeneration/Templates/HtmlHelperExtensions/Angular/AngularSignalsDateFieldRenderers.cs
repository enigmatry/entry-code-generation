using Enigmatry.Entry.CodeGeneration.Configuration.Form.Controls;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Enigmatry.Entry.CodeGeneration.Templates.HtmlHelperExtensions.Angular;

internal static class AngularSignalsDateFieldRenderers
{
    internal static IHtmlContent RenderDatepickerField(this IHtmlHelper htmlHelper, DatepickerFormControl field, FormViewRenderContext context)
    {
        var pickerElementId = $"picker_{context.MemberName(field, "")}";
        return htmlHelper.Raw(
            $"@if (!isHidden('{context.Key(field)}', {field.Visible.ToString().ToLower()})) {{\r\n" +
            $"<mat-form-field {field.FieldClassAttribute()}{field.AppearanceAttribute()}{field.TooltipAttribute(context.EnableI18N)}>\r\n" +
            $"    <mat-label>{{{{ label('{context.Key(field)}') }}}}</mat-label>\r\n" +
            $"    <input matInput [matDatepicker]=\"{pickerElementId}\" formControlName=\"{field.PropertyName}\"{field.PlaceholderAttribute(context.EnableI18N)}>\r\n" +
            $"    <mat-datepicker-toggle matIconSuffix [for]=\"{pickerElementId}\"></mat-datepicker-toggle>\r\n" +
            $"    <mat-datepicker #{pickerElementId}></mat-datepicker>\r\n" +
            field.HintLine(context.EnableI18N) +
            htmlHelper.RenderValidationErrors(field, context) +
            $"</mat-form-field>\r\n" +
            $"}}\r\n");
    }

    internal static IHtmlContent RenderDateTimePickerField(this IHtmlHelper htmlHelper, DateTimePickerFormControl field, FormViewRenderContext context)
    {
        var pickerElementId = $"picker_{context.MemberName(field, "")}";
        return htmlHelper.Raw(
            $"@if (!isHidden('{context.Key(field)}', {field.Visible.ToString().ToLower()})) {{\r\n" +
            $"<mat-form-field {field.FieldClassAttribute()}{field.AppearanceAttribute()}{field.TooltipAttribute(context.EnableI18N)}>\r\n" +
            $"    <mat-label>{{{{ label('{context.Key(field)}') }}}}</mat-label>\r\n" +
            $"    <input matInput [matDatetimepicker]=\"{pickerElementId}\" formControlName=\"{field.PropertyName}\"{field.PlaceholderAttribute(context.EnableI18N)}>\r\n" +
            $"    <mat-datetimepicker-toggle matIconSuffix [for]=\"{pickerElementId}\"></mat-datetimepicker-toggle>\r\n" +
            $"    <mat-datetimepicker #{pickerElementId}></mat-datetimepicker>\r\n" +
            field.HintLine(context.EnableI18N) +
            htmlHelper.RenderValidationErrors(field, context) +
            $"</mat-form-field>\r\n" +
            $"}}\r\n");
    }
}
