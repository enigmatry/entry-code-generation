using Enigmatry.Entry.CodeGeneration.Configuration.Form.Controls;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Enigmatry.Entry.CodeGeneration.Templates.HtmlHelperExtensions.Angular;

internal static class AngularSignalsSelectFieldRenderers
{
    internal static IHtmlContent RenderSelectField(this IHtmlHelper htmlHelper, SelectFormControl field, bool enableI18N) =>
        htmlHelper.Raw(
            $"@if (!isHidden('{field.PropertyName}', {field.Visible.ToString().ToLower()})) {{\r\n" +
            $"<mat-form-field {field.FieldClassAttribute()}{field.AppearanceAttribute()}{field.TooltipAttribute(enableI18N)}>\r\n" +
            $"    <mat-label>{{{{ label('{field.PropertyName}') }}}}</mat-label>\r\n" +
            $"    <mat-select formControlName=\"{field.PropertyName}\"{field.MetadataAttributes()}>\r\n" +
            $"        @for (option of {field.PropertyName}Options(); track option.value) {{\r\n" +
            $"            <mat-option [value]=\"option.value\">{{{{option.displayName}}}}</mat-option>\r\n" +
            $"        }}\r\n" +
            $"    </mat-select>\r\n" +
            field.HintLine(enableI18N) +
            htmlHelper.RenderValidationErrors(field, enableI18N) +
            $"</mat-form-field>\r\n" +
            $"}}\r\n");

    internal static IHtmlContent RenderMultiSelectField(this IHtmlHelper htmlHelper, MultiSelectFormControl field, bool enableI18N)
    {
        var selectAllOption = field.Options.SelectAllOption;
        var selectAllLine = selectAllOption != null
            ? $"        <mat-option (click)=\"toggleSelectAll('{field.PropertyName}', {field.PropertyName}Options())\"{selectAllOption.DisplayName.I18NAttribute(enableI18N)}>{selectAllOption.DisplayName.Value}</mat-option>\r\n"
            : "";
        return htmlHelper.Raw(
            $"@if (!isHidden('{field.PropertyName}', {field.Visible.ToString().ToLower()})) {{\r\n" +
            $"<mat-form-field {field.FieldClassAttribute()}{field.AppearanceAttribute()}{field.TooltipAttribute(enableI18N)}>\r\n" +
            $"    <mat-label>{{{{ label('{field.PropertyName}') }}}}</mat-label>\r\n" +
            $"    <mat-select formControlName=\"{field.PropertyName}\" multiple{field.MetadataAttributes()}>\r\n" +
            selectAllLine +
            $"        @for (option of {field.PropertyName}Options(); track option.value) {{\r\n" +
            $"            <mat-option [value]=\"option.value\">{{{{option.displayName}}}}</mat-option>\r\n" +
            $"        }}\r\n" +
            $"    </mat-select>\r\n" +
            field.HintLine(enableI18N) +
            htmlHelper.RenderValidationErrors(field, enableI18N) +
            $"</mat-form-field>\r\n" +
            $"}}\r\n");
    }

    internal static IHtmlContent RenderRadioGroupField(this IHtmlHelper htmlHelper, RadioGroupFormControl field, bool enableI18N) =>
        htmlHelper.Raw(
            $"@if (!isHidden('{field.PropertyName}', {field.Visible.ToString().ToLower()})) {{\r\n" +
            $"<div {field.FieldClassAttribute()}{field.TooltipAttribute(enableI18N)}>\r\n" +
            $"    <label id=\"{field.PropertyName}RadioGroupLabel\" class=\"entry-radio-group-label\">{{{{ label('{field.PropertyName}') }}}}</label>\r\n" +
            $"    <mat-radio-group formControlName=\"{field.PropertyName}\" aria-labelledby=\"{field.PropertyName}RadioGroupLabel\">\r\n" +
            $"        @for (option of {field.PropertyName}Options(); track option.value) {{\r\n" +
            $"            <mat-radio-button [value]=\"option.value\">{{{{option.displayName}}}}</mat-radio-button>\r\n" +
            $"        }}\r\n" +
            $"    </mat-radio-group>\r\n" +
            $"</div>\r\n" +
            $"}}\r\n");

    internal static IHtmlContent RenderMultiCheckboxField(this IHtmlHelper htmlHelper, MultiCheckboxFormControl field, bool enableI18N) =>
        htmlHelper.Raw(
            $"@if (!isHidden('{field.PropertyName}', {field.Visible.ToString().ToLower()})) {{\r\n" +
            $"<div {field.FieldClassAttribute()}{field.TooltipAttribute(enableI18N)}>\r\n" +
            $"    @for (option of {field.PropertyName}Options(); track option.value) {{\r\n" +
            $"        <mat-checkbox [checked]=\"isOptionSelected('{field.PropertyName}', option.value)\"\r\n" +
            $"                      [disabled]=\"isDisabled('{field.PropertyName}', {field.Readonly.ToString().ToLower()})\"\r\n" +
            $"                      (change)=\"toggleOption('{field.PropertyName}', option.value, $event.checked)\">{{{{option.displayName}}}}</mat-checkbox>\r\n" +
            $"    }}\r\n" +
            $"</div>\r\n" +
            $"}}\r\n");

    internal static IHtmlContent RenderAutocompleteField(this IHtmlHelper htmlHelper, AutocompleteFormControl field, bool enableI18N)
    {
        var propertyNameCapitalized = AngularSignalsFormModelExtensions.Capitalize(field.PropertyName);
        return htmlHelper.Raw(
            $"@if (!isHidden('{field.PropertyName}', {field.Visible.ToString().ToLower()})) {{\r\n" +
            $"<mat-form-field {field.FieldClassAttribute()}{field.AppearanceAttribute()}{field.TooltipAttribute(enableI18N)}>\r\n" +
            $"    <mat-label>{{{{ label('{field.PropertyName}') }}}}</mat-label>\r\n" +
            $"    <input type=\"text\" matInput formControlName=\"{field.PropertyName}\"\r\n" +
            $"        [matAutocomplete]=\"{field.PropertyName}Auto\"{field.PlaceholderAttribute(enableI18N)}\r\n" +
            $"        [readonly]=\"isDisabled('{field.PropertyName}', {field.Readonly.ToString().ToLower()})\">\r\n" +
            $"    <mat-autocomplete #{field.PropertyName}Auto=\"matAutocomplete\"\r\n" +
            $"        [autoActiveFirstOption]=\"true\"\r\n" +
            $"        [displayWith]=\"display{propertyNameCapitalized}\">\r\n" +
            $"        @for (option of {field.PropertyName}FilteredOptions(); track option.value) {{\r\n" +
            $"            <mat-option [value]=\"option.value\">{{{{option.displayName}}}}</mat-option>\r\n" +
            $"        }}\r\n" +
            $"    </mat-autocomplete>\r\n" +
            field.HintLine(enableI18N) +
            htmlHelper.RenderValidationErrors(field, enableI18N) +
            $"</mat-form-field>\r\n" +
            $"}}\r\n");
    }
}
