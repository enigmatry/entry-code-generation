using Enigmatry.Entry.CodeGeneration.Configuration;
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
        // cdkFocusInitial only takes effect inside a CDK focus trap; the native attribute covers plain pages.
        var autofocusAttribute = field.Autofocus ? " autofocus cdkFocusInitial" : "";

        return htmlHelper.Raw(
            $"@if (!{context.IsHiddenCall(field)}) {{\r\n" +
            $"<mat-form-field {field.FieldClassAttribute()}{field.AppearanceAttribute()}{floatLabelAttribute}{field.TooltipAttribute(context.EnableI18N)}>\r\n" +
            $"    <mat-label>{{{{ {context.LabelCall(field)} }}}}</mat-label>\r\n" +
            $"    <input matInput formControlName=\"{field.PropertyName}\" type=\"{inputType}\"{field.PlaceholderAttribute(context.EnableI18N)}{autocompleteAttribute}{autofocusAttribute}{field.FormatAttribute()}{field.MetadataAttributes()} [readonly]=\"{context.IsDisabledCall(field)}\">\r\n" +
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
        var floatLabelAttribute = field.FloatLabel.HasValue
            ? $" floatLabel=\"{field.FloatLabel!.Value.ToString().ToLower()}\""
            : "";
        var autofocusAttribute = field.Autofocus ? " autofocus cdkFocusInitial" : "";

        return htmlHelper.Raw(
            $"@if (!{context.IsHiddenCall(field)}) {{\r\n" +
            $"<mat-form-field {field.FieldClassAttribute()}{field.AppearanceAttribute()}{floatLabelAttribute}{field.TooltipAttribute(context.EnableI18N)}>\r\n" +
            $"    <mat-label>{{{{ {context.LabelCall(field)} }}}}</mat-label>\r\n" +
            $"    <textarea matInput formControlName=\"{field.PropertyName}\"{field.PlaceholderAttribute(context.EnableI18N)}{rowsAttribute}{colsAttribute}{autocompleteAttribute}{autoResizeAttributes}{autofocusAttribute}{field.FormatAttribute()}{field.MetadataAttributes()} [readonly]=\"{context.IsDisabledCall(field)}\"></textarea>\r\n" +
            field.HintLine(context.EnableI18N) +
            htmlHelper.RenderValidationErrors(field, context) +
            $"</mat-form-field>\r\n" +
            $"}}\r\n");
    }

    internal static IHtmlContent RenderCheckboxField(this IHtmlHelper htmlHelper, CheckboxFormControl field, FormViewRenderContext context) =>
        htmlHelper.Raw(
            $"@if (!{context.IsHiddenCall(field)}) {{\r\n" +
            $"<mat-checkbox formControlName=\"{field.PropertyName}\" {field.FieldClassAttribute()}{field.TooltipAttribute(context.EnableI18N)}{field.MetadataAttributes()}>{{{{ {context.LabelCall(field)} }}}}</mat-checkbox>\r\n" +
            field.HintLine(context.EnableI18N) +
            htmlHelper.RenderValidationErrorsWhenTouched(field, context) +
            $"}}\r\n");

    internal static IHtmlContent RenderRichTextField(this IHtmlHelper htmlHelper, RichTextInputFormControl field, FormViewRenderContext context)
    {
        var editorTagName = $"entry-{field.Editor.ToString().ToLower()}";
        return htmlHelper.Raw(
            $"@if (!{context.IsHiddenCall(field)}) {{\r\n" +
            $"<div {field.FieldClassAttribute()}{field.TooltipAttribute(context.EnableI18N)}>\r\n" +
            field.FieldLabelLine(context) +
            $"    <{editorTagName} formControlName=\"{field.PropertyName}\"{field.PlaceholderAttribute(context.EnableI18N)}></{editorTagName}>\r\n" +
            field.HintLine(context.EnableI18N) +
            htmlHelper.RenderValidationErrorsWhenTouched(field, context) +
            $"</div>\r\n" +
            $"}}\r\n");
    }

    // Custom control components must expose a readonly input (alongside the WithImport requirement);
    // the reactive-forms disabled state is handled by the component-level effects.
    internal static IHtmlContent RenderCustomField(this IHtmlHelper htmlHelper, CustomFormControl field, FormViewRenderContext context)
    {
        var classes = $"entry-{field.PropertyName.Kebaberize()}-field {field.ControlTypeName}";
        return htmlHelper.Raw(
            $"@if (!{context.IsHiddenCall(field)}) {{\r\n" +
            $"<div class=\"{classes}\"{field.ConditionalClassBindings()}{field.TooltipAttribute(context.EnableI18N)}>\r\n" +
            field.FieldLabelLine(context) +
            $"    <{field.ControlTypeName} formControlName=\"{field.PropertyName}\"{field.PlaceholderAttribute(context.EnableI18N)}{field.MetadataAttributes()}" +
            $" [readonly]=\"{context.IsDisabledCall(field)}\"></{field.ControlTypeName}>\r\n" +
            field.HintLine(context.EnableI18N) +
            htmlHelper.RenderValidationErrorsWhenTouched(field, context) +
            $"</div>\r\n" +
            $"}}\r\n");
    }

}
