using Enigmatry.Entry.CodeGeneration.Configuration;
using Enigmatry.Entry.CodeGeneration.Configuration.Form.Controls;
using Enigmatry.Entry.CodeGeneration.Configuration.Form.Controls.Array;
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
            $"        (click)=\"buttonClick.emit('{button.PropertyName}')\"{button.Text.I18NAttribute(enableI18N)}>{button.Text.Value}</button>\r\n");

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
            _ => htmlHelper.RenderGenericField(field)
        };
    }

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
