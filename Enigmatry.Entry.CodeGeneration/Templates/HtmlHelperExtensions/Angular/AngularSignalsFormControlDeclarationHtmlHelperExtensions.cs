using System.Globalization;
using Enigmatry.Entry.CodeGeneration.Configuration.Form;
using Enigmatry.Entry.CodeGeneration.Configuration.Form.Controls;
using Enigmatry.Entry.CodeGeneration.Configuration.Form.Controls.Array;
using Humanizer;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Enigmatry.Entry.CodeGeneration.Templates.HtmlHelperExtensions.Angular;

public static class AngularSignalsFormControlDeclarationHtmlHelperExtensions
{
    public static string GetTypeScriptTypeAnnotation(this FormControl control) => control switch
    {
        MultiSelectFormControl => "<unknown[] | null>",
        MultiCheckboxFormControl => "<unknown[] | null>",
        RichTextInputFormControl => "<string | null>",
        InputControlBase input when input.IsNumeric() => "<number | null>",
        InputControlBase => "<string | null>",
        CheckboxFormControl => "<boolean>",
        _ => ""
    };

    public static string GetInitialValue(this FormControl control) => control switch
    {
        InputControlBase { DefaultValue: not null } input when input.IsNumeric() => AsJsLiteral(input.DefaultValue),
        InputControlBase input when input.DefaultValue != null => $"'{EscapeSingleQuotes(input.DefaultValue)}'",
        CheckboxFormControl { DefaultValue: not null } checkbox => checkbox.DefaultValue.Value.ToString().ToLower(),
        CheckboxFormControl => "false",
        SelectFormControl { DefaultValue: not null } select => AsJsLiteral(select.DefaultValue),
        RadioGroupFormControl { DefaultValue: not null } radioGroup => AsJsLiteral(radioGroup.DefaultValue),
        DatepickerFormControl { DefaultValue: not null } datepicker => $"'{datepicker.DefaultValue.Value.ToString("O", CultureInfo.InvariantCulture)}'",
        DateTimePickerFormControl { DefaultValue: not null } dateTimePicker => $"'{dateTimePicker.DefaultValue.Value.ToString("O", CultureInfo.InvariantCulture)}'",
        _ => "null"
    };

    private static string AsJsLiteral(string value) =>
        value is "true" or "false" || decimal.TryParse(value, NumberStyles.Number, CultureInfo.InvariantCulture, out _)
            ? value
            : $"'{EscapeSingleQuotes(value)}'";

    private static string EscapeSingleQuotes(string value) => value.Replace("'", "\\'");

    public static IHtmlContent FormControlDeclaration(this IHtmlHelper htmlHelper, FormControl control, string indent = "        ")
    {
        if (control is ArrayFormControl array)
        {
            return htmlHelper.Raw($"{indent}{array.PropertyName}: new FormArray<FormGroup>([]),\r\n");
        }

        var typeAnnotation = control.GetTypeScriptTypeAnnotation();
        var initialValue = control.GetInitialValue();
        var valueExpression = control.Readonly
            ? $"{{ value: {initialValue}, disabled: true }}"
            : initialValue;

        var options = BuildFormControlOptions(htmlHelper, control);

        var declaration = options.Count > 0
            ? $"{indent}{control.PropertyName}: new FormControl{typeAnnotation}({valueExpression}, {{ {String.Join(", ", options)} }}),"
            : $"{indent}{control.PropertyName}: new FormControl{typeAnnotation}({valueExpression}),";

        return htmlHelper.Raw(declaration + "\r\n");
    }

    public static IHtmlContent AllFormControlDeclarations(this IHtmlHelper htmlHelper, FormComponentModel model) =>
        htmlHelper.Raw(String.Concat(model.FlatFormControls().Select(control => htmlHelper.FormControlDeclaration(control).ToString())));

    private static List<string> BuildFormControlOptions(IHtmlHelper htmlHelper, FormControl control)
    {
        var options = new List<string>();

        if (control.ValidationRules.Any())
        {
            options.Add($"validators: {htmlHelper.AngularValidators(control)}");
        }

        if (control.Validators.Any())
        {
            var asyncValidatorParts = String.Join(", ",
                control.Validators.Select(validator => $"this.asyncValidatorResolver('{validator.Name.Camelize()}')"));
            options.Add($"asyncValidators: this.asyncValidatorResolver ? [{asyncValidatorParts}] : []");
        }

        if (control.ValueUpdateTrigger.HasValue && control.ValueUpdateTrigger.Value != ValueUpdateTrigger.OnChange)
        {
            var updateOnValue = control.ValueUpdateTrigger.Value switch
            {
                ValueUpdateTrigger.OnBlur => "blur",
                ValueUpdateTrigger.OnSubmit => "submit",
                _ => "change"
            };
            options.Add($"updateOn: '{updateOnValue}'");
        }

        return options;
    }
}
