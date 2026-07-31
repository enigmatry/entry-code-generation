using Enigmatry.Entry.CodeGeneration.Configuration.Form.Controls;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Enigmatry.Entry.CodeGeneration.Templates.HtmlHelperExtensions.Angular;

internal static class AngularSignalsSelectFieldRenderers
{
    /// <summary>
    /// Option markup for a mat-select / mat-autocomplete: a flat @for, or one nested inside
    /// mat-optgroup elements when the control configures option groups. Options without a group
    /// value land in a leading/trailing group that renders without an optgroup wrapper.
    /// </summary>
    private static string OptionElements(SelectControlBase field, FormViewRenderContext context, string indent, string memberSuffix = "")
    {
        var optionsMember = context.MemberName(field, $"{memberSuffix}Options");
        if (!field.RendersOptionGroups())
        {
            return
                $"{indent}@for (option of {optionsMember}(); track option.value) {{\r\n" +
                $"{indent}    <mat-option [value]=\"option.value\">{{{{option.displayName}}}}</mat-option>\r\n" +
                $"{indent}}}\r\n";
        }

        return
            $"{indent}@for (optionGroup of {context.MemberName(field, $"{memberSuffix}OptionGroups")}(); track optionGroup.group) {{\r\n" +
            $"{indent}    @if (optionGroup.group) {{\r\n" +
            $"{indent}    <mat-optgroup [label]=\"optionGroup.group\">\r\n" +
            $"{indent}        @for (option of optionGroup.options; track option.value) {{\r\n" +
            $"{indent}        <mat-option [value]=\"option.value\">{{{{option.displayName}}}}</mat-option>\r\n" +
            $"{indent}        }}\r\n" +
            $"{indent}    </mat-optgroup>\r\n" +
            $"{indent}    }} @else {{\r\n" +
            $"{indent}        @for (option of optionGroup.options; track option.value) {{\r\n" +
            $"{indent}        <mat-option [value]=\"option.value\">{{{{option.displayName}}}}</mat-option>\r\n" +
            $"{indent}        }}\r\n" +
            $"{indent}    }}\r\n" +
            $"{indent}}}\r\n";
    }

    internal static IHtmlContent RenderSelectField(this IHtmlHelper htmlHelper, SelectFormControl field, FormViewRenderContext context) =>
        htmlHelper.Raw(
            $"@if (!{context.IsHiddenCall(field)}) {{\r\n" +
            $"<mat-form-field {field.FieldClassAttribute()}{field.AppearanceAttribute()}{field.TooltipAttribute(context.EnableI18N)}>\r\n" +
            $"    <mat-label>{{{{ {context.LabelCall(field)} }}}}</mat-label>\r\n" +
            $"    <mat-select formControlName=\"{field.PropertyName}\"{field.MetadataAttributes()}>\r\n" +
            OptionElements(field, context, "        ") +
            $"    </mat-select>\r\n" +
            field.HintLine(context.EnableI18N) +
            htmlHelper.RenderValidationErrors(field, context) +
            $"</mat-form-field>\r\n" +
            $"}}\r\n");

    internal static IHtmlContent RenderMultiSelectField(this IHtmlHelper htmlHelper, MultiSelectFormControl field, FormViewRenderContext context)
    {
        var selectAllOption = field.Options.SelectAllOption;
        // onSelectionChange fires after Material finished its own selection processing, so the
        // helper's full-value overwrite deterministically wins (a plain (click) races with it).
        var selectAllLine = selectAllOption != null
            ? $"        <mat-option (onSelectionChange)=\"$event.isUserInput && toggleSelectAll({context.FormGroupAccessor}.get('{field.PropertyName}'), {context.MemberName(field, "Options")}())\"{selectAllOption.DisplayName.I18NAttribute(context.EnableI18N)}>{selectAllOption.DisplayName.Value.EscapeHtmlText()}</mat-option>\r\n"
            : "";
        return htmlHelper.Raw(
            $"@if (!{context.IsHiddenCall(field)}) {{\r\n" +
            $"<mat-form-field {field.FieldClassAttribute()}{field.AppearanceAttribute()}{field.TooltipAttribute(context.EnableI18N)}>\r\n" +
            $"    <mat-label>{{{{ {context.LabelCall(field)} }}}}</mat-label>\r\n" +
            $"    <mat-select formControlName=\"{field.PropertyName}\" multiple{field.MetadataAttributes()}>\r\n" +
            selectAllLine +
            OptionElements(field, context, "        ") +
            $"    </mat-select>\r\n" +
            field.HintLine(context.EnableI18N) +
            htmlHelper.RenderValidationErrors(field, context) +
            $"</mat-form-field>\r\n" +
            $"}}\r\n");
    }

    internal static IHtmlContent RenderRadioGroupField(this IHtmlHelper htmlHelper, RadioGroupFormControl field, FormViewRenderContext context) =>
        htmlHelper.Raw(
            $"@if (!{context.IsHiddenCall(field)}) {{\r\n" +
            $"<div {field.FieldClassAttribute()}{field.TooltipAttribute(context.EnableI18N)}>\r\n" +
            $"    <label id=\"{context.MemberName(field, "RadioGroupLabel")}\" class=\"entry-radio-group-label\">{{{{ {context.LabelCall(field)} }}}}</label>\r\n" +
            $"    <mat-radio-group formControlName=\"{field.PropertyName}\" aria-labelledby=\"{context.MemberName(field, "RadioGroupLabel")}\">\r\n" +
            $"        @for (option of {context.MemberName(field, "Options")}(); track option.value) {{\r\n" +
            $"            <mat-radio-button [value]=\"option.value\">{{{{option.displayName}}}}</mat-radio-button>\r\n" +
            $"        }}\r\n" +
            $"    </mat-radio-group>\r\n" +
            htmlHelper.RenderValidationErrorsWhenTouched(field, context) +
            $"</div>\r\n" +
            $"}}\r\n");

    internal static IHtmlContent RenderMultiCheckboxField(this IHtmlHelper htmlHelper, MultiCheckboxFormControl field, FormViewRenderContext context) =>
        htmlHelper.Raw(
            $"@if (!{context.IsHiddenCall(field)}) {{\r\n" +
            $"<div {field.FieldClassAttribute()}{field.TooltipAttribute(context.EnableI18N)}>\r\n" +
            $"    @for (option of {context.MemberName(field, "Options")}(); track option.value) {{\r\n" +
            $"        <mat-checkbox [checked]=\"isOptionSelected({context.FormGroupAccessor}.get('{field.PropertyName}'), option.value)\"\r\n" +
            $"                      [disabled]=\"{context.IsDisabledCall(field)}\"\r\n" +
            $"                      (change)=\"toggleOption({context.FormGroupAccessor}.get('{field.PropertyName}'), option.value, $event.checked)\">{{{{option.displayName}}}}</mat-checkbox>\r\n" +
            $"    }}\r\n" +
            htmlHelper.RenderValidationErrorsWhenTouched(field, context) +
            $"</div>\r\n" +
            $"}}\r\n");

    internal static IHtmlContent RenderAutocompleteField(this IHtmlHelper htmlHelper, AutocompleteFormControl field, FormViewRenderContext context) =>
        htmlHelper.Raw(
            $"@if (!{context.IsHiddenCall(field)}) {{\r\n" +
            $"<mat-form-field {field.FieldClassAttribute()}{field.AppearanceAttribute()}{field.TooltipAttribute(context.EnableI18N)}>\r\n" +
            $"    <mat-label>{{{{ {context.LabelCall(field)} }}}}</mat-label>\r\n" +
            $"    <input type=\"text\" matInput formControlName=\"{field.PropertyName}\"\r\n" +
            $"        [matAutocomplete]=\"{field.PropertyName}Auto\"{field.PlaceholderAttribute(context.EnableI18N)}\r\n" +
            $"        [readonly]=\"{context.IsDisabledCall(field)}\">\r\n" +
            $"    <mat-autocomplete #{field.PropertyName}Auto=\"matAutocomplete\"\r\n" +
            $"        [autoActiveFirstOption]=\"true\"\r\n" +
            $"        [displayWith]=\"display{AngularSignalsFormModelExtensions.Capitalize(field.PropertyName)}\">\r\n" +
            OptionElements(field, context, "        ", "Filtered") +
            $"    </mat-autocomplete>\r\n" +
            field.HintLine(context.EnableI18N) +
            htmlHelper.RenderValidationErrors(field, context) +
            $"</mat-form-field>\r\n" +
            $"}}\r\n");
}
