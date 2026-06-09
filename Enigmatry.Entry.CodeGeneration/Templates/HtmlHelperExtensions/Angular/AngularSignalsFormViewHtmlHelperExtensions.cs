using Enigmatry.Entry.CodeGeneration.Configuration;
using Enigmatry.Entry.CodeGeneration.Configuration.Form.Controls;
using Enigmatry.Entry.CodeGeneration.Configuration.Form.Controls.Array;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Enigmatry.Entry.CodeGeneration.Templates.HtmlHelperExtensions.Angular;

public static class AngularSignalsFormViewHtmlHelperExtensions
{
    public static IHtmlContent RenderFormControls(this IHtmlHelper htmlHelper, IEnumerable<FormControl> controls) =>
        htmlHelper.Raw(String.Concat(controls.Select(control => htmlHelper.RenderFormControl(control).ToString())));

    private static IHtmlContent RenderFormControl(this IHtmlHelper htmlHelper, FormControl control) => control switch
    {
        FormControlGroup group => htmlHelper.RenderFormControlGroup(group),
        ButtonFormControl button => htmlHelper.RenderFormButton(button),
        _ => htmlHelper.RenderFormField(control)
    };

    private static IHtmlContent RenderFormControlGroup(this IHtmlHelper htmlHelper, FormControlGroup group)
    {
        var staticClasses = group.ClassNames.Values
            .Where(classNameEntry => classNameEntry.When == ApplyWhen.Always)
            .Aggregate("entry-field-group", (current, classNameEntry) => current + $" {classNameEntry.Value}");
        var classBindings = String.Join("", group.ClassNames.Values
            .Where(classNameEntry => classNameEntry.When != ApplyWhen.Always)
            .Select(classNameEntry => classNameEntry.When == ApplyWhen.FormIsReadonly
                ? $" [class.{classNameEntry.Value}]=\"isReadonly()\""
                : $" [class.{classNameEntry.Value}]=\"!isReadonly()\""));
        var innerContent = htmlHelper.RenderFormControls(group.FormControls).ToString();
        return htmlHelper.Raw($"<div class=\"{staticClasses}\"{classBindings}>\r\n{innerContent}</div>\r\n");
    }

    private static IHtmlContent RenderFormButton(this IHtmlHelper htmlHelper, ButtonFormControl button) =>
        htmlHelper.Raw(
            $"<button mat-button type=\"button\"\r\n" +
            $"        class=\"{button.StackedClasses()}\"\r\n" +
            $"        [disabled]=\"isDisabled('{button.PropertyName}', {button.Readonly.ToString().ToLower()})\"\r\n" +
            $"        (click)=\"buttonClick.emit('{button.PropertyName}')\">{button.Text.Value}</button>\r\n");

    private static IHtmlContent RenderFormField(this IHtmlHelper htmlHelper, FormControl field)
    {
        if (!field.Visible)
        {
            return htmlHelper.Raw("");
        }

        return field switch
        {
            ArrayFormControl arrayControl => htmlHelper.RenderArrayField(arrayControl),
            RichTextInputFormControl richTextField => htmlHelper.RenderRichTextField(richTextField),
            DatepickerFormControl datepickerField => htmlHelper.RenderDatepickerField(datepickerField),
            DateTimePickerFormControl dateTimePickerField => htmlHelper.RenderDateTimePickerField(dateTimePickerField),
            MultiSelectFormControl multiSelectField => htmlHelper.RenderMultiSelectField(multiSelectField),
            SelectFormControl selectField => htmlHelper.RenderSelectField(selectField),
            MultiCheckboxFormControl multiCheckboxField => htmlHelper.RenderMultiCheckboxField(multiCheckboxField),
            RadioGroupFormControl radioGroupField => htmlHelper.RenderRadioGroupField(radioGroupField),
            CheckboxFormControl checkboxField => htmlHelper.RenderCheckboxField(checkboxField),
            InputControlBase inputField => htmlHelper.RenderInputField(inputField),
            _ => htmlHelper.RenderGenericField(field)
        };
    }

    private static IHtmlContent RenderInputField(this IHtmlHelper htmlHelper, InputControlBase field)
    {
        var inputType = field.Type ?? "text";
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
        var validationErrors = String.Concat(field.ValidationRules.Select(validationRule =>
            $"@if (form.get('{field.PropertyName}')?.hasError('{validationRule.FormlyRuleName}')) {{\r\n" +
            $"    <mat-error>{(validationRule.HasCustomMessage ? validationRule.CustomMessage : validationRule.FormlyValidationMessage)}</mat-error>\r\n" +
            $"}}"));

        return htmlHelper.Raw(
            $"@if (!isHidden('{field.PropertyName}', {field.Visible.ToString().ToLower()})) {{\r\n" +
            $"<mat-form-field class=\"{field.StackedClasses()}\"{appearanceAttribute}{floatLabelAttribute}>\r\n" +
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
            $"<mat-checkbox formControlName=\"{field.PropertyName}\" class=\"{field.StackedClasses()}\">{field.Label.Value}</mat-checkbox>\r\n" +
            $"}}\r\n");

    private static IHtmlContent RenderSelectField(this IHtmlHelper htmlHelper, SelectFormControl field)
    {
        var appearanceAttribute = field.Appearance.HasValue
            ? $" appearance=\"{field.Appearance!.Value.ToString().ToLower()}\""
            : "";
        return htmlHelper.Raw(
            $"@if (!isHidden('{field.PropertyName}', {field.Visible.ToString().ToLower()})) {{\r\n" +
            $"<mat-form-field class=\"{field.StackedClasses()}\"{appearanceAttribute}>\r\n" +
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
            $"<mat-form-field class=\"{field.StackedClasses()}\"{appearanceAttribute}>\r\n" +
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
            $"<mat-radio-group formControlName=\"{field.PropertyName}\" class=\"{field.StackedClasses()}\">\r\n" +
            $"    @for (option of {field.PropertyName}Options(); track option.value) {{\r\n" +
            $"        <mat-radio-button [value]=\"option.value\">{{{{option.displayName}}}}</mat-radio-button>\r\n" +
            $"    }}\r\n" +
            $"</mat-radio-group>\r\n" +
            $"}}\r\n");

    private static IHtmlContent RenderMultiCheckboxField(this IHtmlHelper htmlHelper, MultiCheckboxFormControl field) =>
        htmlHelper.Raw(
            $"@if (!isHidden('{field.PropertyName}', {field.Visible.ToString().ToLower()})) {{\r\n" +
            $"<div class=\"{field.StackedClasses()}\">\r\n" +
            $"    @for (option of {field.PropertyName}Options(); track option.value) {{\r\n" +
            $"        <mat-checkbox [value]=\"option.value\">{{{{option.displayName}}}}</mat-checkbox>\r\n" +
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
            $"<mat-form-field class=\"{field.StackedClasses()}\"{appearanceAttribute}>\r\n" +
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
            $"<mat-form-field class=\"{field.StackedClasses()}\"{appearanceAttribute}>\r\n" +
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
            $"<{editorTagName} formControlName=\"{field.PropertyName}\" class=\"{field.StackedClasses()}\"></{editorTagName}>\r\n" +
            $"}}\r\n");
    }

    private static IHtmlContent RenderGenericField(this IHtmlHelper htmlHelper, FormControl field) =>
        htmlHelper.Raw(
            $"@if (!isHidden('{field.PropertyName}', {field.Visible.ToString().ToLower()})) {{\r\n" +
            $"<input formControlName=\"{field.PropertyName}\" class=\"{field.StackedClasses()}\">\r\n" +
            $"}}\r\n");

    private static IHtmlContent RenderArrayField(this IHtmlHelper htmlHelper, ArrayFormControl arrayControl)
    {
        var group = (FormControlGroup)arrayControl.FormControlGroup;
        var propertyName = arrayControl.PropertyName;
        var innerControls = htmlHelper.RenderFormControls(group.FormControls).ToString();
        return htmlHelper.Raw(
            $"@if (!isHidden('{propertyName}', {arrayControl.Visible.ToString().ToLower()})) {{\r\n" +
            $"<ng-container formArrayName=\"{propertyName}\">\r\n" +
            $"    @for (control of form.get('{propertyName}')?.controls ?? []; track $index) {{\r\n" +
            $"        <ng-container [formGroupName]=\"$index\">\r\n" +
            innerControls +
            $"        </ng-container>\r\n" +
            $"    }}\r\n" +
            $"</ng-container>\r\n" +
            $"}}\r\n");
    }
}
