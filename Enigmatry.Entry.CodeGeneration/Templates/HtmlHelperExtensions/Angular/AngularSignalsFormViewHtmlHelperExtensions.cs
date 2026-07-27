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
        ButtonFormControl button => htmlHelper.RenderFormButton(button),
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

    private static IHtmlContent RenderFormButton(this IHtmlHelper htmlHelper, ButtonFormControl button) =>
        htmlHelper.Raw(
            $"<button mat-button type=\"button\"\r\n" +
            $"        {button.FieldClassAttribute()}\r\n" +
            $"        [disabled]=\"isDisabled('{button.PropertyName}', {button.Readonly.ToString().ToLower()})\"\r\n" +
            $"        (click)=\"buttonClick.emit('{button.PropertyName}')\">{button.Text.Value}</button>\r\n");

    private static IHtmlContent RenderFormField(this IHtmlHelper htmlHelper, FormControl field, bool enableI18N)
    {
        if (!field.Visible)
        {
            return htmlHelper.Raw("");
        }

        return field switch
        {
            ArrayFormControl arrayControl => htmlHelper.RenderArrayField(arrayControl, enableI18N),
            RichTextInputFormControl richTextField => htmlHelper.RenderRichTextField(richTextField),
            DatepickerFormControl datepickerField => htmlHelper.RenderDatepickerField(datepickerField),
            DateTimePickerFormControl dateTimePickerField => htmlHelper.RenderDateTimePickerField(dateTimePickerField),
            MultiSelectFormControl multiSelectField => htmlHelper.RenderMultiSelectField(multiSelectField),
            SelectFormControl selectField => htmlHelper.RenderSelectField(selectField),
            MultiCheckboxFormControl multiCheckboxField => htmlHelper.RenderMultiCheckboxField(multiCheckboxField),
            RadioGroupFormControl radioGroupField => htmlHelper.RenderRadioGroupField(radioGroupField),
            TextareaFormControl textareaField => htmlHelper.RenderTextareaField(textareaField, enableI18N),
            AutocompleteFormControl autocompleteField => htmlHelper.RenderAutocompleteField(autocompleteField),
            CheckboxFormControl checkboxField => htmlHelper.RenderCheckboxField(checkboxField),
            InputControlBase inputField => htmlHelper.RenderInputField(inputField, enableI18N),
            CustomFormControl customField => htmlHelper.RenderCustomField(customField),
            _ => htmlHelper.RenderGenericField(field)
        };
    }

    private static IHtmlContent RenderTextareaField(this IHtmlHelper htmlHelper, TextareaFormControl field, bool enableI18N)
    {
        var placeholderAttribute = field.Placeholder.Value.HasContent()
            ? $" [placeholder]=\"'{field.Placeholder.Value}'\""
            : "";
        var rowsAttribute = field.Rows > 0 ? $" rows=\"{field.Rows}\"" : "";
        var colsAttribute = field.Cols > 0 ? $" cols=\"{field.Cols}\"" : "";
        var autoResizeAttributes = field.AutoResize
            ? " cdkTextareaAutosize" +
              (field.AutoResizeMinRows > 0 ? $" cdkAutosizeMinRows=\"{field.AutoResizeMinRows}\"" : "") +
              (field.AutoResizeMaxRows > 0 ? $" cdkAutosizeMaxRows=\"{field.AutoResizeMaxRows}\"" : "")
            : "";
        var autocompleteAttribute = field.ShouldAutocomplete == false ? " autocomplete=\"off\"" : "";
        var validationErrors = htmlHelper.RenderValidationErrors(field, enableI18N);

        return htmlHelper.Raw(
            $"@if (!isHidden('{field.PropertyName}', {field.Visible.ToString().ToLower()})) {{\r\n" +
            $"<mat-form-field {field.FieldClassAttribute()}>\r\n" +
            $"    <mat-label>{field.Label.Value}</mat-label>\r\n" +
            $"    <textarea matInput formControlName=\"{field.PropertyName}\"{placeholderAttribute}{rowsAttribute}{colsAttribute}{autocompleteAttribute}{autoResizeAttributes} [readonly]=\"isDisabled('{field.PropertyName}', {field.Readonly.ToString().ToLower()})\"></textarea>\r\n" +
            validationErrors +
            $"</mat-form-field>\r\n" +
            $"}}\r\n");
    }

    private static IHtmlContent RenderAutocompleteField(this IHtmlHelper htmlHelper, AutocompleteFormControl field)
    {
        var appearanceAttribute = field.Appearance.HasValue
            ? $" appearance=\"{field.Appearance!.Value.ToString().ToLower()}\""
            : "";
        var placeholderAttribute = field.Placeholder.Value.HasContent()
            ? $" [placeholder]=\"'{field.Placeholder.Value}'\""
            : "";
        var propertyNameCapitalized = Char.ToUpper(field.PropertyName[0]) + field.PropertyName.Substring(1);
        return htmlHelper.Raw(
            $"@if (!isHidden('{field.PropertyName}', {field.Visible.ToString().ToLower()})) {{\r\n" +
            $"<mat-form-field {field.FieldClassAttribute()}{appearanceAttribute}>\r\n" +
            $"    <mat-label>{field.Label.Value}</mat-label>\r\n" +
            $"    <input type=\"text\" matInput formControlName=\"{field.PropertyName}\"\r\n" +
            $"        [matAutocomplete]=\"{field.PropertyName}Auto\"{placeholderAttribute}\r\n" +
            $"        [readonly]=\"isDisabled('{field.PropertyName}', {field.Readonly.ToString().ToLower()})\">\r\n" +
            $"    <mat-autocomplete #{field.PropertyName}Auto=\"matAutocomplete\"\r\n" +
            $"        [autoActiveFirstOption]=\"true\"\r\n" +
            $"        [displayWith]=\"display{propertyNameCapitalized}\">\r\n" +
            $"        @for (option of {field.PropertyName}Options(); track option.value) {{\r\n" +
            $"            <mat-option [value]=\"option.value\">{{{{option.displayName}}}}</mat-option>\r\n" +
            $"        }}\r\n" +
            $"    </mat-autocomplete>\r\n" +
            $"</mat-form-field>\r\n" +
            $"}}\r\n");
    }

    private static IHtmlContent RenderInputField(this IHtmlHelper htmlHelper, InputControlBase field, bool enableI18N)
    {
        var inputType = field.Type ?? (field.IsNumeric() ? "number" : "text");
        var appearanceAttribute = field.Appearance.HasValue
            ? $" appearance=\"{field.Appearance!.Value.ToString().ToLower()}\""
            : "";
        var floatLabelAttribute = field.FloatLabel.HasValue
            ? $" floatLabel=\"{field.FloatLabel!.Value.ToString().ToLower()}\""
            : "";
        var placeholderAttribute = field.Placeholder.Value.HasContent()
            ? $" [placeholder]=\"'{field.Placeholder.Value}'\""
            : "";
        var autocompleteAttribute = field.ShouldAutocomplete == false ? " autocomplete=\"off\"" : "";
        var autofocusAttribute = field.Autofocus ? " cdkFocusInitial" : "";
        var hintLine = field.Hint.Value.HasContent()
            ? $"    <mat-hint>{field.Hint.Value}</mat-hint>\r\n"
            : "";
        var validationErrors = htmlHelper.RenderValidationErrors(field, enableI18N);

        return htmlHelper.Raw(
            $"@if (!isHidden('{field.PropertyName}', {field.Visible.ToString().ToLower()})) {{\r\n" +
            $"<mat-form-field {field.FieldClassAttribute()}{appearanceAttribute}{floatLabelAttribute}>\r\n" +
            $"    <mat-label>{field.Label.Value}</mat-label>\r\n" +
            $"    <input matInput formControlName=\"{field.PropertyName}\" type=\"{inputType}\"{placeholderAttribute}{autocompleteAttribute}{autofocusAttribute} [readonly]=\"isDisabled('{field.PropertyName}', {field.Readonly.ToString().ToLower()})\">\r\n" +
            hintLine +
            validationErrors +
            $"</mat-form-field>\r\n" +
            $"}}\r\n");
    }

    private static IHtmlContent RenderCheckboxField(this IHtmlHelper htmlHelper, CheckboxFormControl field) =>
        htmlHelper.Raw(
            $"@if (!isHidden('{field.PropertyName}', {field.Visible.ToString().ToLower()})) {{\r\n" +
            $"<mat-checkbox formControlName=\"{field.PropertyName}\" {field.FieldClassAttribute()}>{field.Label.Value}</mat-checkbox>\r\n" +
            $"}}\r\n");

    private static IHtmlContent RenderSelectField(this IHtmlHelper htmlHelper, SelectFormControl field)
    {
        var appearanceAttribute = field.Appearance.HasValue
            ? $" appearance=\"{field.Appearance!.Value.ToString().ToLower()}\""
            : "";
        return htmlHelper.Raw(
            $"@if (!isHidden('{field.PropertyName}', {field.Visible.ToString().ToLower()})) {{\r\n" +
            $"<mat-form-field {field.FieldClassAttribute()}{appearanceAttribute}>\r\n" +
            $"    <mat-label>{field.Label.Value}</mat-label>\r\n" +
            $"    <mat-select formControlName=\"{field.PropertyName}\">\r\n" +
            $"        @for (option of {field.PropertyName}Options(); track option.value) {{\r\n" +
            $"            <mat-option [value]=\"option.value\">{{{{option.displayName}}}}</mat-option>\r\n" +
            $"        }}\r\n" +
            $"    </mat-select>\r\n" +
            $"</mat-form-field>\r\n" +
            $"}}\r\n");
    }

    private static IHtmlContent RenderMultiSelectField(this IHtmlHelper htmlHelper, MultiSelectFormControl field)
    {
        var selectAllOption = field.Options.SelectAllOption;
        var appearanceAttribute = field.Appearance.HasValue
            ? $" appearance=\"{field.Appearance!.Value.ToString().ToLower()}\""
            : "";
        var selectAllLine = selectAllOption != null
            ? $"        <mat-option>{selectAllOption.DisplayName.Value}</mat-option>\r\n"
            : "";
        return htmlHelper.Raw(
            $"@if (!isHidden('{field.PropertyName}', {field.Visible.ToString().ToLower()})) {{\r\n" +
            $"<mat-form-field {field.FieldClassAttribute()}{appearanceAttribute}>\r\n" +
            $"    <mat-label>{field.Label.Value}</mat-label>\r\n" +
            $"    <mat-select formControlName=\"{field.PropertyName}\" multiple>\r\n" +
            selectAllLine +
            $"        @for (option of {field.PropertyName}Options(); track option.value) {{\r\n" +
            $"            <mat-option [value]=\"option.value\">{{{{option.displayName}}}}</mat-option>\r\n" +
            $"        }}\r\n" +
            $"    </mat-select>\r\n" +
            $"</mat-form-field>\r\n" +
            $"}}\r\n");
    }

    private static IHtmlContent RenderRadioGroupField(this IHtmlHelper htmlHelper, RadioGroupFormControl field) =>
        htmlHelper.Raw(
            $"@if (!isHidden('{field.PropertyName}', {field.Visible.ToString().ToLower()})) {{\r\n" +
            $"<mat-radio-group formControlName=\"{field.PropertyName}\" {field.FieldClassAttribute()}>\r\n" +
            $"    @for (option of {field.PropertyName}Options(); track option.value) {{\r\n" +
            $"        <mat-radio-button [value]=\"option.value\">{{{{option.displayName}}}}</mat-radio-button>\r\n" +
            $"    }}\r\n" +
            $"</mat-radio-group>\r\n" +
            $"}}\r\n");

    private static IHtmlContent RenderMultiCheckboxField(this IHtmlHelper htmlHelper, MultiCheckboxFormControl field) =>
        htmlHelper.Raw(
            $"@if (!isHidden('{field.PropertyName}', {field.Visible.ToString().ToLower()})) {{\r\n" +
            $"<div {field.FieldClassAttribute()}>\r\n" +
            $"    @for (option of {field.PropertyName}Options(); track option.value) {{\r\n" +
            $"        <mat-checkbox [checked]=\"isOptionSelected('{field.PropertyName}', option.value)\"\r\n" +
            $"                      [disabled]=\"isDisabled('{field.PropertyName}', {field.Readonly.ToString().ToLower()})\"\r\n" +
            $"                      (change)=\"toggleOption('{field.PropertyName}', option.value, $event.checked)\">{{{{option.displayName}}}}</mat-checkbox>\r\n" +
            $"    }}\r\n" +
            $"</div>\r\n" +
            $"}}\r\n");

    private static IHtmlContent RenderDatepickerField(this IHtmlHelper htmlHelper, DatepickerFormControl field)
    {
        var pickerElementId = $"picker_{field.PropertyName}";
        var appearanceAttribute = field.Appearance.HasValue
            ? $" appearance=\"{field.Appearance!.Value.ToString().ToLower()}\""
            : "";
        var placeholderAttribute = field.Placeholder.Value.HasContent()
            ? $" [placeholder]=\"'{field.Placeholder.Value}'\""
            : "";
        return htmlHelper.Raw(
            $"@if (!isHidden('{field.PropertyName}', {field.Visible.ToString().ToLower()})) {{\r\n" +
            $"<mat-form-field {field.FieldClassAttribute()}{appearanceAttribute}>\r\n" +
            $"    <mat-label>{field.Label.Value}</mat-label>\r\n" +
            $"    <input matInput [matDatepicker]=\"{pickerElementId}\" formControlName=\"{field.PropertyName}\"{placeholderAttribute}>\r\n" +
            $"    <mat-datepicker-toggle matIconSuffix [for]=\"{pickerElementId}\"></mat-datepicker-toggle>\r\n" +
            $"    <mat-datepicker #{pickerElementId}></mat-datepicker>\r\n" +
            $"</mat-form-field>\r\n" +
            $"}}\r\n");
    }

    private static IHtmlContent RenderDateTimePickerField(this IHtmlHelper htmlHelper, DateTimePickerFormControl field)
    {
        var pickerElementId = $"picker_{field.PropertyName}";
        var appearanceAttribute = field.Appearance.HasValue
            ? $" appearance=\"{field.Appearance!.Value.ToString().ToLower()}\""
            : "";
        var placeholderAttribute = field.Placeholder.Value.HasContent()
            ? $" [placeholder]=\"'{field.Placeholder.Value}'\""
            : "";
        return htmlHelper.Raw(
            $"@if (!isHidden('{field.PropertyName}', {field.Visible.ToString().ToLower()})) {{\r\n" +
            $"<mat-form-field {field.FieldClassAttribute()}{appearanceAttribute}>\r\n" +
            $"    <mat-label>{field.Label.Value}</mat-label>\r\n" +
            $"    <input matInput [matDatetimepicker]=\"{pickerElementId}\" formControlName=\"{field.PropertyName}\"{placeholderAttribute}>\r\n" +
            $"    <mat-datetimepicker-toggle matIconSuffix [for]=\"{pickerElementId}\"></mat-datetimepicker-toggle>\r\n" +
            $"    <mat-datetimepicker #{pickerElementId}></mat-datetimepicker>\r\n" +
            $"</mat-form-field>\r\n" +
            $"}}\r\n");
    }

    private static IHtmlContent RenderRichTextField(this IHtmlHelper htmlHelper, RichTextInputFormControl field)
    {
        var editorTagName = $"entry-{field.Editor.ToString().ToLower()}";
        return htmlHelper.Raw(
            $"@if (!isHidden('{field.PropertyName}', {field.Visible.ToString().ToLower()})) {{\r\n" +
            $"<{editorTagName} formControlName=\"{field.PropertyName}\" {field.FieldClassAttribute()}></{editorTagName}>\r\n" +
            $"}}\r\n");
    }

    private static IHtmlContent RenderCustomField(this IHtmlHelper htmlHelper, CustomFormControl field)
    {
        var metadataAttributes = String.Concat(field.Metadata.Select(kv => $" {kv.Key}=\"{kv.Value}\""));
        var classes = $"entry-{field.PropertyName.Kebaberize()}-field {field.ControlTypeName}";
        return htmlHelper.Raw(
            $"@if (!isHidden('{field.PropertyName}', {field.Visible.ToString().ToLower()})) {{\r\n" +
            $"<{field.ControlTypeName} formControlName=\"{field.PropertyName}\" class=\"{classes}\"{field.ConditionalClassBindings()}{metadataAttributes}" +
            $" [readonly]=\"isDisabled('{field.PropertyName}', {field.Readonly.ToString().ToLower()})\"></{field.ControlTypeName}>\r\n" +
            $"}}\r\n");
    }

    private static string RenderValidationErrors(this IHtmlHelper htmlHelper, FormControl field, bool enableI18N) =>
        String.Concat(field.ValidationRules.Select(validationRule =>
        {
            var rawMessage = validationRule.HasCustomMessage ? validationRule.CustomMessage : validationRule.FormlyValidationMessage;
            var message = field.ResolvedValidationMessage(validationRule);

            // A message that got field-specific values interpolated into it can no longer share
            // a translation id with other fields, so it gets the same per-field id shape that
            // FormControl.ApplyValidationConfiguration mints.
            var translationId = message == rawMessage
                ? validationRule.MessageTranslationId
                : $"{field.ComponentInfo.Feature.Name.Kebaberize()}" +
                  $".{field.ComponentInfo.Name.Kebaberize()}" +
                  $".{field.PropertyName.Kebaberize()}" +
                  $".{validationRule.FormlyRuleName.Kebaberize()}";
            var i18nAttribute = enableI18N && translationId.HasContent() ? $" i18n=\"@@{translationId}\"" : "";

            return
                $"@if (form.get('{field.PropertyName}')?.hasError('{validationRule.AngularErrorKey()}')) {{\r\n" +
                $"    <mat-error{i18nAttribute}>{message}</mat-error>\r\n" +
                $"}}";
        }));

    // Angular's built-in Validators.minLength/maxLength report their errors under
    // all-lowercase keys, unlike the camelCase rule names used for the validator factories.
    private static string AngularErrorKey(this IFormlyValidationRule validationRule) => validationRule.FormlyRuleName switch
    {
        "minLength" => "minlength",
        "maxLength" => "maxlength",
        _ => validationRule.FormlyRuleName
    };

    // Default rule messages carry Formly-era runtime interpolations such as
    // "${field?.templateOptions?.label}:property-name:"; the label and the rule
    // values are known at generation time, so they are resolved into plain text here.
    private static string ResolvedValidationMessage(this FormControl field, IFormlyValidationRule validationRule)
    {
        var message = validationRule.HasCustomMessage ? validationRule.CustomMessage : validationRule.FormlyValidationMessage;

        return Regex.Replace(message, @"\$\{field\?\.templateOptions\?\.(\w+)\}:[\w-]+:", match =>
        {
            var propertyReference = match.Groups[1].Value;
            if (propertyReference == "label")
            {
                return field.Label.Value;
            }

            var valueText = validationRule.FormlyTemplateOptions
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
        var staticClasses = $"entry-{field.PropertyName.Kebaberize()}-field entry-{field.FormlyType.Kebaberize()}";
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

    private static IHtmlContent RenderGenericField(this IHtmlHelper htmlHelper, FormControl field) =>
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
