using System.Globalization;
using System.Text.RegularExpressions;
using Enigmatry.Entry.CodeGeneration.Configuration;
using Enigmatry.Entry.CodeGeneration.Configuration.Form.Controls;
using Enigmatry.Entry.CodeGeneration.Configuration.Form.Controls.Array;
using Enigmatry.Entry.CodeGeneration.Validation.ValidationRules;
using Humanizer;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Enigmatry.Entry.CodeGeneration.Templates.HtmlHelperExtensions.Angular;

public static class AngularSignalsFormViewHtmlHelperExtensions
{
    public static IHtmlContent RenderFormControls(this IHtmlHelper htmlHelper, IEnumerable<FormControl> controls, bool enableI18N) =>
        htmlHelper.Raw(String.Concat(controls.Select(control => htmlHelper.RenderFormControl(control, enableI18N).ToString())));

    private static IHtmlContent RenderFormControl(this IHtmlHelper htmlHelper, FormControl control, bool enableI18N) => control switch
    {
        FormControlGroup group => htmlHelper.RenderFormControlGroup(group, enableI18N),
        ButtonFormControl button => htmlHelper.RenderFormButton(button, enableI18N),
        _ => htmlHelper.RenderFormField(control, enableI18N)
    };

    private static IHtmlContent RenderFormControlGroup(this IHtmlHelper htmlHelper, FormControlGroup group, bool enableI18N)
    {
        var staticClasses = group.ClassNames.Values
            .Where(classNameEntry => classNameEntry.When == ApplyWhen.Always)
            .Aggregate("entry-field-group", (current, classNameEntry) => current + $" {classNameEntry.Value}");
        var innerContent = htmlHelper.RenderFormControls(group.FormControls, enableI18N).ToString();
        return htmlHelper.Raw($"<div class=\"{staticClasses}\"{group.ConditionalClassBindings()}>\r\n{innerContent}</div>\r\n");
    }

    private static IHtmlContent RenderFormButton(this IHtmlHelper htmlHelper, ButtonFormControl button, bool enableI18N) =>
        htmlHelper.Raw(
            $"<button mat-button type=\"button\"\r\n" +
            $"        {button.FieldClassAttribute()}\r\n" +
            $"        [disabled]=\"isDisabled('{button.PropertyName}', {button.Readonly.ToString().ToLower()})\"\r\n" +
            $"        (click)=\"buttonClick.emit('{button.PropertyName}')\"{ElementI18NAttribute(button.Text, enableI18N)}>{button.Text.Value}</button>\r\n");

    private static IHtmlContent RenderFormField(this IHtmlHelper htmlHelper, FormControl field, bool enableI18N)
    {
        if (!field.Visible)
        {
            return htmlHelper.Raw("");
        }

        return field switch
        {
            ArrayFormControl arrayControl => htmlHelper.RenderArrayField(arrayControl, enableI18N),
            RichTextInputFormControl richTextField => htmlHelper.RenderRichTextField(richTextField, enableI18N),
            DatepickerFormControl datepickerField => htmlHelper.RenderDatepickerField(datepickerField, enableI18N),
            DateTimePickerFormControl dateTimePickerField => htmlHelper.RenderDateTimePickerField(dateTimePickerField, enableI18N),
            MultiSelectFormControl multiSelectField => htmlHelper.RenderMultiSelectField(multiSelectField, enableI18N),
            SelectFormControl selectField => htmlHelper.RenderSelectField(selectField, enableI18N),
            MultiCheckboxFormControl multiCheckboxField => htmlHelper.RenderMultiCheckboxField(multiCheckboxField, enableI18N),
            RadioGroupFormControl radioGroupField => htmlHelper.RenderRadioGroupField(radioGroupField, enableI18N),
            TextareaFormControl textareaField => htmlHelper.RenderTextareaField(textareaField, enableI18N),
            AutocompleteFormControl autocompleteField => htmlHelper.RenderAutocompleteField(autocompleteField, enableI18N),
            CheckboxFormControl checkboxField => htmlHelper.RenderCheckboxField(checkboxField, enableI18N),
            InputControlBase inputField => htmlHelper.RenderInputField(inputField, enableI18N),
            CustomFormControl customField => htmlHelper.RenderCustomField(customField, enableI18N),
            _ => htmlHelper.RenderGenericField(field, enableI18N)
        };
    }

    private static IHtmlContent RenderTextareaField(this IHtmlHelper htmlHelper, TextareaFormControl field, bool enableI18N)
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

    private static IHtmlContent RenderAutocompleteField(this IHtmlHelper htmlHelper, AutocompleteFormControl field, bool enableI18N)
    {
        var propertyNameCapitalized = Char.ToUpper(field.PropertyName[0]) + field.PropertyName.Substring(1);
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

    private static IHtmlContent RenderInputField(this IHtmlHelper htmlHelper, InputControlBase field, bool enableI18N)
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

    private static IHtmlContent RenderCheckboxField(this IHtmlHelper htmlHelper, CheckboxFormControl field, bool enableI18N) =>
        htmlHelper.Raw(
            $"@if (!isHidden('{field.PropertyName}', {field.Visible.ToString().ToLower()})) {{\r\n" +
            $"<mat-checkbox formControlName=\"{field.PropertyName}\" {field.FieldClassAttribute()}{field.TooltipAttribute(enableI18N)}>{{{{ label('{field.PropertyName}') }}}}</mat-checkbox>\r\n" +
            $"}}\r\n");

    private static IHtmlContent RenderSelectField(this IHtmlHelper htmlHelper, SelectFormControl field, bool enableI18N) =>
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

    private static IHtmlContent RenderMultiSelectField(this IHtmlHelper htmlHelper, MultiSelectFormControl field, bool enableI18N)
    {
        var selectAllOption = field.Options.SelectAllOption;
        var selectAllLine = selectAllOption != null
            ? $"        <mat-option (click)=\"toggleSelectAll('{field.PropertyName}', {field.PropertyName}Options())\"{ElementI18NAttribute(selectAllOption.DisplayName, enableI18N)}>{selectAllOption.DisplayName.Value}</mat-option>\r\n"
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

    private static IHtmlContent RenderRadioGroupField(this IHtmlHelper htmlHelper, RadioGroupFormControl field, bool enableI18N) =>
        htmlHelper.Raw(
            $"@if (!isHidden('{field.PropertyName}', {field.Visible.ToString().ToLower()})) {{\r\n" +
            $"<mat-radio-group formControlName=\"{field.PropertyName}\" {field.FieldClassAttribute()}{field.TooltipAttribute(enableI18N)}>\r\n" +
            $"    @for (option of {field.PropertyName}Options(); track option.value) {{\r\n" +
            $"        <mat-radio-button [value]=\"option.value\">{{{{option.displayName}}}}</mat-radio-button>\r\n" +
            $"    }}\r\n" +
            $"</mat-radio-group>\r\n" +
            $"}}\r\n");

    private static IHtmlContent RenderMultiCheckboxField(this IHtmlHelper htmlHelper, MultiCheckboxFormControl field, bool enableI18N) =>
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

    private static IHtmlContent RenderDatepickerField(this IHtmlHelper htmlHelper, DatepickerFormControl field, bool enableI18N)
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

    private static IHtmlContent RenderDateTimePickerField(this IHtmlHelper htmlHelper, DateTimePickerFormControl field, bool enableI18N)
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

    private static IHtmlContent RenderRichTextField(this IHtmlHelper htmlHelper, RichTextInputFormControl field, bool enableI18N)
    {
        var editorTagName = $"entry-{field.Editor.ToString().ToLower()}";
        return htmlHelper.Raw(
            $"@if (!isHidden('{field.PropertyName}', {field.Visible.ToString().ToLower()})) {{\r\n" +
            $"<{editorTagName} formControlName=\"{field.PropertyName}\" {field.FieldClassAttribute()}{field.TooltipAttribute(enableI18N)}></{editorTagName}>\r\n" +
            $"}}\r\n");
    }

    private static IHtmlContent RenderCustomField(this IHtmlHelper htmlHelper, CustomFormControl field, bool enableI18N)
    {
        var classes = $"entry-{field.PropertyName.Kebaberize()}-field {field.ControlTypeName}";
        return htmlHelper.Raw(
            $"@if (!isHidden('{field.PropertyName}', {field.Visible.ToString().ToLower()})) {{\r\n" +
            $"<{field.ControlTypeName} formControlName=\"{field.PropertyName}\" class=\"{classes}\"{field.ConditionalClassBindings()}{field.MetadataAttributes()}{field.TooltipAttribute(enableI18N)}" +
            $" [readonly]=\"isDisabled('{field.PropertyName}', {field.Readonly.ToString().ToLower()})\"></{field.ControlTypeName}>\r\n" +
            $"}}\r\n");
    }

    private static string RenderValidationErrors(this IHtmlHelper htmlHelper, FormControl field, bool enableI18N) =>
        String.Concat(field.ValidationRules.Select(validationRule =>
        {
            var rawMessage = validationRule.HasCustomMessage ? validationRule.CustomMessage : validationRule.ValidationMessage;
            var message = field.ResolvedValidationMessage(validationRule);

            // A message that got field-specific values interpolated into it can no longer share
            // a translation id with other fields, so it gets the same per-field id shape that
            // FormControl.ApplyValidationConfiguration mints.
            var translationId = message == rawMessage
                ? validationRule.MessageTranslationId
                : $"{field.ComponentInfo.Feature.Name.Kebaberize()}" +
                  $".{field.ComponentInfo.Name.Kebaberize()}" +
                  $".{field.PropertyName.Kebaberize()}" +
                  $".{validationRule.RuleName.Kebaberize()}";
            var i18nAttribute = enableI18N && translationId.HasContent() ? $" i18n=\"@@{translationId}\"" : "";

            return
                $"@if (form.get('{field.PropertyName}')?.hasError('{validationRule.AngularErrorKey()}')) {{\r\n" +
                $"    <mat-error{i18nAttribute}>{message}</mat-error>\r\n" +
                $"}}";
        }));

    // Angular's built-in Validators.minLength/maxLength report their errors under
    // all-lowercase keys, unlike the camelCase rule names used for the validator factories.
    private static string AngularErrorKey(this IFormlyValidationRule validationRule) => validationRule.RuleName switch
    {
        "minLength" => "minlength",
        "maxLength" => "maxlength",
        _ => validationRule.RuleName
    };

    // Default rule messages carry Formly-era runtime interpolations such as
    // "${field?.templateOptions?.label}:property-name:"; the label and the rule
    // values are known at generation time, so they are resolved into plain text here.
    private static string ResolvedValidationMessage(this FormControl field, IFormlyValidationRule validationRule)
    {
        var message = validationRule.HasCustomMessage ? validationRule.CustomMessage : validationRule.ValidationMessage;

        return Regex.Replace(message, @"\$\{field\?\.templateOptions\?\.(\w+)\}:[\w-]+:", match =>
        {
            var propertyReference = match.Groups[1].Value;
            if (propertyReference == "label")
            {
                return field.Label.Value;
            }

            var valueText = validationRule.TemplateOptions
                .FirstOrDefault(templateOption => templateOption.StartsWith($"{propertyReference}: ", StringComparison.Ordinal))
                ?[(propertyReference.Length + 2)..];

            return valueText == null ? match.Value : EvaluateNumericExpression(valueText);
        });
    }

    private static string EvaluateNumericExpression(string expression)
    {
        var parts = expression.Split(new[] { '+', '-' }, StringSplitOptions.TrimEntries);
        if (parts.Length == 2
            && decimal.TryParse(parts[0], NumberStyles.Number, CultureInfo.InvariantCulture, out var left)
            && decimal.TryParse(parts[1], NumberStyles.Number, CultureInfo.InvariantCulture, out var right))
        {
            var result = expression.Contains('+') ? left + right : left - right;
            return result.ToString(CultureInfo.InvariantCulture);
        }

        return expression;
    }

    private static string FieldClassAttribute(this FormControl field)
    {
        var staticClasses = $"entry-{field.PropertyName.Kebaberize()}-field entry-{field.ControlType.Kebaberize()}";
        staticClasses = field.ClassNames.Values
            .Where(classNameEntry => classNameEntry.When == ApplyWhen.Always)
            .Aggregate(staticClasses, (current, classNameEntry) => current + $" {classNameEntry.Value}");

        return $"class=\"{staticClasses}\"{field.ConditionalClassBindings()}";
    }

    private static string ConditionalClassBindings(this FormControl field) =>
        String.Join("", field.ClassNames.Values
            .Where(classNameEntry => classNameEntry.When != ApplyWhen.Always)
            .Select(classNameEntry => classNameEntry.When == ApplyWhen.FormIsReadonly
                ? $" [class.{classNameEntry.Value}]=\"isReadonly()\""
                : $" [class.{classNameEntry.Value}]=\"!isReadonly()\""));

    // Only 'fill' and 'outline' are valid mat-form-field appearances; the legacy
    // Formly-era values (standard/legacy/none) fall back to the Material default.
    private static string AppearanceAttribute(this FormControl field) => field.Appearance switch
    {
        FormControlAppearance.Fill => " appearance=\"fill\"",
        FormControlAppearance.Outline => " appearance=\"outline\"",
        _ => ""
    };

    private static string PlaceholderAttribute(this FormControl field, bool enableI18N) =>
        field.Placeholder.Value.HasContent()
            ? $" placeholder=\"{field.Placeholder.Value}\"{AttributeI18NAttribute("placeholder", field.Placeholder, enableI18N)}"
            : "";

    private static string TooltipAttribute(this FormControl field, bool enableI18N) =>
        field.Tooltip.Value.HasContent()
            ? $" matTooltip=\"{field.Tooltip.Value}\"{AttributeI18NAttribute("matTooltip", field.Tooltip, enableI18N)}"
            : "";

    private static string HintLine(this FormControl field, bool enableI18N) =>
        field.Hint.Value.HasContent()
            ? $"    <mat-hint{ElementI18NAttribute(field.Hint, enableI18N)}>{field.Hint.Value}</mat-hint>\r\n"
            : "";

    private static string MetadataAttributes(this FormControl field) =>
        String.Concat(field.Metadata.Select(metadataEntry => $" {metadataEntry.Key}=\"{metadataEntry.Value}\""));

    private static string ElementI18NAttribute(I18NString text, bool enableI18N) =>
        enableI18N && text.Key.HasContent() && text.Value.HasContent()
            ? $" i18n=\"@@{text.Key}\""
            : "";

    private static string AttributeI18NAttribute(string attributeName, I18NString text, bool enableI18N) =>
        enableI18N && text.Key.HasContent() && text.Value.HasContent()
            ? $" i18n-{attributeName}=\"@@{text.Key}\""
            : "";

    private static IHtmlContent RenderGenericField(this IHtmlHelper htmlHelper, FormControl field, bool enableI18N) =>
        htmlHelper.Raw(
            $"@if (!isHidden('{field.PropertyName}', {field.Visible.ToString().ToLower()})) {{\r\n" +
            $"<input formControlName=\"{field.PropertyName}\" {field.FieldClassAttribute()}>\r\n" +
            $"}}\r\n");

    private static IHtmlContent RenderArrayField(this IHtmlHelper htmlHelper, ArrayFormControl arrayControl, bool enableI18N)
    {
        var group = (FormControlGroup)arrayControl.FormControlGroup;
        var propertyName = arrayControl.PropertyName;
        var innerControls = htmlHelper.RenderFormControls(group.FormControls, enableI18N).ToString();
        return htmlHelper.Raw(
            $"@if (!isHidden('{propertyName}', {arrayControl.Visible.ToString().ToLower()})) {{\r\n" +
            $"<ng-container formArrayName=\"{propertyName}\">\r\n" +
            $"    @for (control of form.controls.{propertyName}.controls; track $index) {{\r\n" +
            $"        <ng-container [formGroupName]=\"$index\">\r\n" +
            innerControls +
            $"        </ng-container>\r\n" +
            $"    }}\r\n" +
            $"</ng-container>\r\n" +
            $"}}\r\n");
    }
}
