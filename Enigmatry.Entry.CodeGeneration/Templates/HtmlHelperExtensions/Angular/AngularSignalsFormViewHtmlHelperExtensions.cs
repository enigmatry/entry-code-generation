using Enigmatry.Entry.CodeGeneration.Configuration;
using Enigmatry.Entry.CodeGeneration.Configuration.Form.Controls;
using Enigmatry.Entry.CodeGeneration.Configuration.Form.Controls.Array;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Enigmatry.Entry.CodeGeneration.Templates.HtmlHelperExtensions.Angular;

public static class AngularSignalsFormViewHtmlHelperExtensions
{
    public static IHtmlContent RenderFormControls(this IHtmlHelper htmlHelper, IEnumerable<FormControl> controls, FormViewRenderContext context) =>
        htmlHelper.Raw(String.Concat(controls.Select(control => htmlHelper.RenderFormControl(control, context).ToString())));

    private static IHtmlContent RenderFormControl(this IHtmlHelper htmlHelper, FormControl control, FormViewRenderContext context) => control switch
    {
        FormControlGroup group => htmlHelper.RenderFormControlGroup(group, context),
        ButtonFormControl button => htmlHelper.RenderFormButton(button, context.EnableI18N),
        _ => htmlHelper.RenderFormField(control, context)
    };

    private static IHtmlContent RenderFormControlGroup(this IHtmlHelper htmlHelper, FormControlGroup group, FormViewRenderContext context)
    {
        var staticClasses = group.ClassNames.Values
            .Where(classNameEntry => classNameEntry.When == ApplyWhen.Always)
            .Aggregate("entry-field-group", (current, classNameEntry) => current + $" {classNameEntry.Value}");
        var innerContent = htmlHelper.RenderFormControls(group.FormControls, context).ToString();
        return htmlHelper.Raw($"<div class=\"{staticClasses}\"{group.ConditionalClassBindings()}>\r\n{innerContent}</div>\r\n");
    }

    private static IHtmlContent RenderFormButton(this IHtmlHelper htmlHelper, ButtonFormControl button, bool enableI18N) =>
        htmlHelper.Raw(
            $"<button mat-button type=\"button\"\r\n" +
            $"        {button.FieldClassAttribute()}\r\n" +
            $"        [disabled]=\"isDisabled('{button.PropertyName}', {button.Readonly.ToString().ToLower()})\"\r\n" +
            $"        (click)=\"buttonClick.emit('{button.PropertyName}')\"{button.Text.I18NAttribute(enableI18N)}>{button.Text.Value}</button>\r\n");

    private static IHtmlContent RenderFormField(this IHtmlHelper htmlHelper, FormControl field, FormViewRenderContext context)
    {
        if (!field.Visible)
        {
            return htmlHelper.Raw("");
        }

        var controlMarkup = field switch
        {
            ArrayFormControl arrayControl => htmlHelper.RenderArrayField(arrayControl, context),
            RichTextInputFormControl richTextField => htmlHelper.RenderRichTextField(richTextField, context),
            DatepickerFormControl datepickerField => htmlHelper.RenderDatepickerField(datepickerField, context),
            DateTimePickerFormControl dateTimePickerField => htmlHelper.RenderDateTimePickerField(dateTimePickerField, context),
            MultiSelectFormControl multiSelectField => htmlHelper.RenderMultiSelectField(multiSelectField, context),
            SelectFormControl selectField => htmlHelper.RenderSelectField(selectField, context),
            MultiCheckboxFormControl multiCheckboxField => htmlHelper.RenderMultiCheckboxField(multiCheckboxField, context),
            RadioGroupFormControl radioGroupField => htmlHelper.RenderRadioGroupField(radioGroupField, context),
            TextareaFormControl textareaField => htmlHelper.RenderTextareaField(textareaField, context),
            AutocompleteFormControl autocompleteField => htmlHelper.RenderAutocompleteField(autocompleteField, context),
            CheckboxFormControl checkboxField => htmlHelper.RenderCheckboxField(checkboxField, context),
            InputControlBase inputField => htmlHelper.RenderInputField(inputField, context),
            CustomFormControl customField => htmlHelper.RenderCustomField(customField, context),
            _ => htmlHelper.RenderGenericField(field, context)
        };

        return htmlHelper.WrapWithReadonlyDisplay(field, controlMarkup, context);
    }

    private static IHtmlContent RenderGenericField(this IHtmlHelper htmlHelper, FormControl field, FormViewRenderContext context) =>
        htmlHelper.Raw(
            $"@if (!isHidden('{context.Key(field)}', {field.Visible.ToString().ToLower()})) {{\r\n" +
            $"<input formControlName=\"{field.PropertyName}\" {field.FieldClassAttribute()}>\r\n" +
            $"}}\r\n");

    private static IHtmlContent RenderArrayField(this IHtmlHelper htmlHelper, ArrayFormControl arrayControl, FormViewRenderContext context)
    {
        var group = (FormControlGroup)arrayControl.FormControlGroup;
        var propertyName = arrayControl.PropertyName;
        var methodName = AngularSignalsFormModelExtensions.Capitalize(propertyName);
        var itemGroupVariable = $"{propertyName}ItemGroup";
        var itemContext = context with
        {
            FormGroupAccessor = itemGroupVariable,
            ControlKeyPrefix = $"{propertyName}.",
            MemberNamePrefix = propertyName
        };
        var innerControls = htmlHelper.RenderFormControls(group.FormControls, itemContext).ToString();
        return htmlHelper.Raw(
            $"@if (!isHidden('{context.Key(arrayControl)}', {arrayControl.Visible.ToString().ToLower()})) {{\r\n" +
            $"<ng-container formArrayName=\"{propertyName}\">\r\n" +
            $"    @for ({itemGroupVariable} of form.controls.{propertyName}.controls; track $index) {{\r\n" +
            $"        <ng-container [formGroupName]=\"$index\">\r\n" +
            innerControls +
            $"            @if (!isReadonly()) {{\r\n" +
            $"            <button mat-button type=\"button\" class=\"entry-array-remove-button\" (click)=\"remove{methodName}Item($index)\"{arrayControl.RemoveButtonLabel.I18NAttribute(context.EnableI18N)}>{arrayControl.RemoveButtonLabel.Value}</button>\r\n" +
            $"            }}\r\n" +
            $"        </ng-container>\r\n" +
            $"    }}\r\n" +
            $"    @if (!isReadonly()) {{\r\n" +
            $"    <button mat-button type=\"button\" class=\"entry-array-add-button\" (click)=\"add{methodName}Item()\"{arrayControl.AddButtonLabel.I18NAttribute(context.EnableI18N)}>{arrayControl.AddButtonLabel.Value}</button>\r\n" +
            $"    }}\r\n" +
            $"</ng-container>\r\n" +
            $"}}\r\n");
    }
}
