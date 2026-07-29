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
        ButtonFormControl button => htmlHelper.RenderFormButton(button, context),
        _ => htmlHelper.RenderFormField(control, context)
    };

    private static IHtmlContent RenderFormControlGroup(this IHtmlHelper htmlHelper, FormControlGroup group, FormViewRenderContext context)
    {
        // The CreateUiSection type has no Formly type registry to resolve against anymore;
        // it is kept as a CSS class so consumers keep their styling hook.
        var baseClasses = group.WrapperElement.HasContent() ? $"entry-field-group {group.WrapperElement}" : "entry-field-group";
        var staticClasses = group.ClassNames.Values
            .Where(classNameEntry => classNameEntry.When == ApplyWhen.Always)
            .Aggregate(baseClasses, (current, classNameEntry) => current + $" {classNameEntry.Value}");
        var labelLine = group.Label.Value.HasContent()
            ? $"    <label class=\"entry-field-group-label\"{group.Label.I18NAttribute(context.EnableI18N)}>{group.Label.Value.EscapeHtmlText()}</label>\r\n"
            : "";
        var hintLine = group.Hint.Value.HasContent()
            ? $"    <span class=\"entry-field-group-hint\"{group.Hint.I18NAttribute(context.EnableI18N)}>{group.Hint.Value.EscapeHtmlText()}</span>\r\n"
            : "";
        var innerContent = htmlHelper.RenderFormControls(group.FormControls, context).ToString();
        return htmlHelper.Raw($"<div class=\"{staticClasses}\"{group.ConditionalClassBindings()}>\r\n{labelLine}{innerContent}{hintLine}</div>\r\n");
    }

    private static IHtmlContent RenderFormButton(this IHtmlHelper htmlHelper, ButtonFormControl button, FormViewRenderContext context)
    {
        if (!button.Visible)
        {
            return htmlHelper.Raw("");
        }

        // A mat-* custom control type selects the Material button variant; any other custom type
        // already lands as an entry-<type> CSS class through FieldClassAttribute.
        var buttonVariant = button.ControlTypeName != null && button.ControlTypeName.StartsWith("mat-", StringComparison.Ordinal)
            ? button.ControlTypeName
            : "mat-button";
        return htmlHelper.Raw(
            $"@if (!{context.IsHiddenCall(button)}) {{\r\n" +
            $"<button {buttonVariant} type=\"button\"\r\n" +
            $"        {button.FieldClassAttribute()}{button.TooltipAttribute(context.EnableI18N)}\r\n" +
            $"        [disabled]=\"{context.IsDisabledCall(button)}\"\r\n" +
            $"        (click)=\"buttonClick.emit('{button.PropertyName}')\"{button.Text.I18NAttribute(context.EnableI18N)}>{button.Text.Value.EscapeHtmlText()}</button>\r\n" +
            $"}}\r\n");
    }

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
            $"@if (!{context.IsHiddenCall(field)}) {{\r\n" +
            $"<div {field.FieldClassAttribute()}{field.TooltipAttribute(context.EnableI18N)}>\r\n" +
            field.FieldLabelLine(context) +
            $"    <input formControlName=\"{field.PropertyName}\"{field.PlaceholderAttribute(context.EnableI18N)}{field.MetadataAttributes()} [readonly]=\"{context.IsDisabledCall(field)}\">\r\n" +
            field.HintLine(context.EnableI18N) +
            htmlHelper.RenderValidationErrorsWhenTouched(field, context) +
            $"</div>\r\n" +
            $"}}\r\n");
}
