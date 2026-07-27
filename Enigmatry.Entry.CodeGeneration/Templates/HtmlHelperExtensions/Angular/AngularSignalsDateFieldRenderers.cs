using Enigmatry.Entry.CodeGeneration.Configuration.Form.Controls;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Enigmatry.Entry.CodeGeneration.Templates.HtmlHelperExtensions.Angular;

internal static class AngularSignalsDateFieldRenderers
{
    internal static IHtmlContent RenderDatepickerField(this IHtmlHelper htmlHelper, DatepickerFormControl field, bool enableI18N)
    {
        var pickerElementId = $"picker_{field.PropertyName}";
        return htmlHelper.Raw(
            $"@if (!isHidden('{field.PropertyName}', {field.Visible.ToString().ToLower()})) {{\r\n" +
            $"<mat-form-field {field.FieldClassAttribute()}{field.AppearanceAttribute()}{field.TooltipAttribute(enableI18N)}>\r\n" +
            $"    <mat-label>{{{{ label('{field.PropertyName}') }}}}</mat-label>\r\n" +
            $"    <input matInput [matDatepicker]=\"{pickerElementId}\" formControlName=\"{field.PropertyName}\"{field.PlaceholderAttribute(enableI18N)}>\r\n" +
            $"    <mat-datepicker-toggle matIconSuffix [for]=\"{pickerElementId}\"></mat-datepicker-toggle>\r\n" +
            $"    <mat-datepicker #{pickerElementId}></mat-datepicker>\r\n" +
            field.HintLine(enableI18N) +
            htmlHelper.RenderValidationErrors(field, enableI18N) +
            $"</mat-form-field>\r\n" +
            $"}}\r\n");
    }

    internal static IHtmlContent RenderDateTimePickerField(this IHtmlHelper htmlHelper, DateTimePickerFormControl field, bool enableI18N)
    {
        var pickerElementId = $"picker_{field.PropertyName}";
        return htmlHelper.Raw(
            $"@if (!isHidden('{field.PropertyName}', {field.Visible.ToString().ToLower()})) {{\r\n" +
            $"<mat-form-field {field.FieldClassAttribute()}{field.AppearanceAttribute()}{field.TooltipAttribute(enableI18N)}>\r\n" +
            $"    <mat-label>{{{{ label('{field.PropertyName}') }}}}</mat-label>\r\n" +
            $"    <input matInput [matDatetimepicker]=\"{pickerElementId}\" formControlName=\"{field.PropertyName}\"{field.PlaceholderAttribute(enableI18N)}>\r\n" +
            $"    <mat-datetimepicker-toggle matIconSuffix [for]=\"{pickerElementId}\"></mat-datetimepicker-toggle>\r\n" +
            $"    <mat-datetimepicker #{pickerElementId}></mat-datetimepicker>\r\n" +
            field.HintLine(enableI18N) +
            htmlHelper.RenderValidationErrors(field, enableI18N) +
            $"</mat-form-field>\r\n" +
            $"}}\r\n");
    }
}
