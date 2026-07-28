using Enigmatry.Entry.CodeGeneration.Configuration.Form.Controls;
using Humanizer;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Enigmatry.Entry.CodeGeneration.Templates.HtmlHelperExtensions.Angular;

internal static class AngularSignalsInputFieldRenderers
{
    internal static IHtmlContent RenderInputField(this IHtmlHelper htmlHelper, InputControlBase field, FormViewRenderContext context)
    {
        var inputType = field.Type ?? (field.IsNumeric() ? "number" : "text");
        var floatLabelAttribute = field.FloatLabel.HasValue
            ? $" floatLabel=\"{field.FloatLabel!.Value.ToString().ToLower()}\""
            : "";
        var autocompleteAttribute = field.ShouldAutocomplete == false ? " autocomplete=\"off\"" : "";
        var autofocusAttribute = field.Autofocus ? " cdkFocusInitial" : "";

        return htmlHelper.Raw(
            $"@if (!isHidden('{context.Key(field)}', {field.Visible.ToString().ToLower()})) {{\r\n" +
            $"<mat-form-field {field.FieldClassAttribute()}{field.AppearanceAttribute()}{floatLabelAttribute}{field.TooltipAttribute(context.EnableI18N)}>\r\n" +
            $"    <mat-label>{{{{ label('{context.Key(field)}') }}}}</mat-label>\r\n" +
            $"    <input matInput formControlName=\"{field.PropertyName}\" type=\"{inputType}\"{field.PlaceholderAttribute(context.EnableI18N)}{autocompleteAttribute}{autofocusAttribute}{field.FormatAttribute()} [readonly]=\"isDisabled('{context.Key(field)}', {field.Readonly.ToString().ToLower()})\">\r\n" +
            field.HintLine(context.EnableI18N) +
            htmlHelper.RenderValidationErrors(field, context) +
            $"</mat-form-field>\r\n" +
            $"}}\r\n");
    }

    internal static IHtmlContent RenderTextareaField(this IHtmlHelper htmlHelper, TextareaFormControl field, FormViewRenderContext context)
    {
        var rowsAttribute = field.Rows > 0 ? $" rows=\"{field.Rows}\"" : "";
        var colsAttribute = field.Cols > 0 ? $" cols=\"{field.Cols}\"" : "";
        var autoResizeAttributes = field.AutoResize
            ? " cdkTextareaAutosize" +
              (field.AutoResizeMinRows > 0 ? $" cdkAutosizeMinRows=\"{field.AutoResizeMinRows}\"" : "") +
              (field.AutoResizeMaxRows > 0 ? $" cdkAutosizeMaxRows=\"{field.AutoResizeMaxRows}\"" : "")
            : "";
        var autocompleteAttribute = field.ShouldAutocomplete == false ? " autocomplete=\"off\"" : "";

        return htmlHelper.Raw(
            $"@if (!isHidden('{context.Key(field)}', {field.Visible.ToString().ToLower()})) {{\r\n" +
            $"<mat-form-field {field.FieldClassAttribute()}{field.TooltipAttribute(context.EnableI18N)}>\r\n" +
            $"    <mat-label>{{{{ label('{context.Key(field)}') }}}}</mat-label>\r\n" +
            $"    <textarea matInput formControlName=\"{field.PropertyName}\"{field.PlaceholderAttribute(context.EnableI18N)}{rowsAttribute}{colsAttribute}{autocompleteAttribute}{autoResizeAttributes}{field.FormatAttribute()} [readonly]=\"isDisabled('{context.Key(field)}', {field.Readonly.ToString().ToLower()})\"></textarea>\r\n" +
            field.HintLine(context.EnableI18N) +
            htmlHelper.RenderValidationErrors(field, context) +
            $"</mat-form-field>\r\n" +
            $"}}\r\n");
    }

    internal static IHtmlContent RenderCheckboxField(this IHtmlHelper htmlHelper, CheckboxFormControl field, FormViewRenderContext context) =>
        htmlHelper.Raw(
            $"@if (!isHidden('{context.Key(field)}', {field.Visible.ToString().ToLower()})) {{\r\n" +
            $"<mat-checkbox formControlName=\"{field.PropertyName}\" {field.FieldClassAttribute()}{field.TooltipAttribute(context.EnableI18N)}>{{{{ label('{context.Key(field)}') }}}}</mat-checkbox>\r\n" +
            htmlHelper.RenderValidationErrors(field, context) +
            $"}}\r\n");

    internal static IHtmlContent RenderRichTextField(this IHtmlHelper htmlHelper, RichTextInputFormControl field, FormViewRenderContext context)
    {
        var editorTagName = $"entry-{field.Editor.ToString().ToLower()}";
        return htmlHelper.Raw(
            $"@if (!isHidden('{context.Key(field)}', {field.Visible.ToString().ToLower()})) {{\r\n" +
            $"<{editorTagName} formControlName=\"{field.PropertyName}\" {field.FieldClassAttribute()}{field.TooltipAttribute(context.EnableI18N)}></{editorTagName}>\r\n" +
            htmlHelper.RenderValidationErrors(field, context) +
            $"}}\r\n");
    }

    internal static IHtmlContent RenderCustomField(this IHtmlHelper htmlHelper, CustomFormControl field, FormViewRenderContext context)
    {
        var classes = $"entry-{field.PropertyName.Kebaberize()}-field {field.ControlTypeName}";
        return htmlHelper.Raw(
            $"@if (!isHidden('{context.Key(field)}', {field.Visible.ToString().ToLower()})) {{\r\n" +
            $"<{field.ControlTypeName} formControlName=\"{field.PropertyName}\" class=\"{classes}\"{field.ConditionalClassBindings()}{field.MetadataAttributes()}{field.TooltipAttribute(context.EnableI18N)}" +
            $" [readonly]=\"isDisabled('{context.Key(field)}', {field.Readonly.ToString().ToLower()})\"></{field.ControlTypeName}>\r\n" +
            htmlHelper.RenderValidationErrors(field, context) +
            $"}}\r\n");
    }
}
