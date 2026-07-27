using Enigmatry.Entry.CodeGeneration.Configuration.Form.Controls;
using Humanizer;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Enigmatry.Entry.CodeGeneration.Templates.HtmlHelperExtensions.Angular;

internal static class AngularSignalsInputFieldRenderers
{
    internal static IHtmlContent RenderInputField(this IHtmlHelper htmlHelper, InputControlBase field, bool enableI18N)
    {
        var inputType = field.Type ?? (field.IsNumeric() ? "number" : "text");
        var floatLabelAttribute = field.FloatLabel.HasValue
            ? $" floatLabel=\"{field.FloatLabel!.Value.ToString().ToLower()}\""
            : "";
        var autocompleteAttribute = field.ShouldAutocomplete == false ? " autocomplete=\"off\"" : "";
        var autofocusAttribute = field.Autofocus ? " cdkFocusInitial" : "";

        return htmlHelper.Raw(
            $"@if (!isHidden('{field.PropertyName}', {field.Visible.ToString().ToLower()})) {{\r\n" +
            $"<mat-form-field {field.FieldClassAttribute()}{field.AppearanceAttribute()}{floatLabelAttribute}{field.TooltipAttribute(enableI18N)}>\r\n" +
            $"    <mat-label>{{{{ label('{field.PropertyName}') }}}}</mat-label>\r\n" +
            $"    <input matInput formControlName=\"{field.PropertyName}\" type=\"{inputType}\"{field.PlaceholderAttribute(enableI18N)}{autocompleteAttribute}{autofocusAttribute} [readonly]=\"isDisabled('{field.PropertyName}', {field.Readonly.ToString().ToLower()})\">\r\n" +
            field.HintLine(enableI18N) +
            htmlHelper.RenderValidationErrors(field, enableI18N) +
            $"</mat-form-field>\r\n" +
            $"}}\r\n");
    }

    internal static IHtmlContent RenderTextareaField(this IHtmlHelper htmlHelper, TextareaFormControl field, bool enableI18N)
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
            $"@if (!isHidden('{field.PropertyName}', {field.Visible.ToString().ToLower()})) {{\r\n" +
            $"<mat-form-field {field.FieldClassAttribute()}{field.TooltipAttribute(enableI18N)}>\r\n" +
            $"    <mat-label>{{{{ label('{field.PropertyName}') }}}}</mat-label>\r\n" +
            $"    <textarea matInput formControlName=\"{field.PropertyName}\"{field.PlaceholderAttribute(enableI18N)}{rowsAttribute}{colsAttribute}{autocompleteAttribute}{autoResizeAttributes} [readonly]=\"isDisabled('{field.PropertyName}', {field.Readonly.ToString().ToLower()})\"></textarea>\r\n" +
            field.HintLine(enableI18N) +
            htmlHelper.RenderValidationErrors(field, enableI18N) +
            $"</mat-form-field>\r\n" +
            $"}}\r\n");
    }

    internal static IHtmlContent RenderCheckboxField(this IHtmlHelper htmlHelper, CheckboxFormControl field, bool enableI18N) =>
        htmlHelper.Raw(
            $"@if (!isHidden('{field.PropertyName}', {field.Visible.ToString().ToLower()})) {{\r\n" +
            $"<mat-checkbox formControlName=\"{field.PropertyName}\" {field.FieldClassAttribute()}{field.TooltipAttribute(enableI18N)}>{{{{ label('{field.PropertyName}') }}}}</mat-checkbox>\r\n" +
            $"}}\r\n");

    internal static IHtmlContent RenderRichTextField(this IHtmlHelper htmlHelper, RichTextInputFormControl field, bool enableI18N)
    {
        var editorTagName = $"entry-{field.Editor.ToString().ToLower()}";
        return htmlHelper.Raw(
            $"@if (!isHidden('{field.PropertyName}', {field.Visible.ToString().ToLower()})) {{\r\n" +
            $"<{editorTagName} formControlName=\"{field.PropertyName}\" {field.FieldClassAttribute()}{field.TooltipAttribute(enableI18N)}></{editorTagName}>\r\n" +
            $"}}\r\n");
    }

    internal static IHtmlContent RenderCustomField(this IHtmlHelper htmlHelper, CustomFormControl field, bool enableI18N)
    {
        var classes = $"entry-{field.PropertyName.Kebaberize()}-field {field.ControlTypeName}";
        return htmlHelper.Raw(
            $"@if (!isHidden('{field.PropertyName}', {field.Visible.ToString().ToLower()})) {{\r\n" +
            $"<{field.ControlTypeName} formControlName=\"{field.PropertyName}\" class=\"{classes}\"{field.ConditionalClassBindings()}{field.MetadataAttributes()}{field.TooltipAttribute(enableI18N)}" +
            $" [readonly]=\"isDisabled('{field.PropertyName}', {field.Readonly.ToString().ToLower()})\"></{field.ControlTypeName}>\r\n" +
            $"}}\r\n");
    }
}
