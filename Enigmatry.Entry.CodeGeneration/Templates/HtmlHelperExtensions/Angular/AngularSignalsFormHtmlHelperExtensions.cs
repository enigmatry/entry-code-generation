using Enigmatry.Entry.CodeGeneration.Configuration.Form;
using Enigmatry.Entry.CodeGeneration.Configuration.Form.Controls;
using Enigmatry.Entry.CodeGeneration.Configuration.Form.Controls.Array;
using Enigmatry.Entry.CodeGeneration.Templates.HtmlHelperExtensions.TypeScript;
using Humanizer;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Enigmatry.Entry.CodeGeneration.Templates.HtmlHelperExtensions.Angular;

public static class AngularSignalsFormHtmlHelperExtensions
{
    public static IEnumerable<FormControl> FlatFormControls(this FormComponentModel model) =>
        model.FormControls.FlatFormControls();

    public static IEnumerable<FormControl> FlatFormControls(this IEnumerable<FormControl> controls)
    {
        foreach (var control in controls)
        {
            switch (control)
            {
                case FormControlGroup group:
                    foreach (var child in group.FormControls.FlatFormControls())
                    {
                        yield return child;
                    }
                    break;
                case ButtonFormControl:
                    break;
                default:
                    yield return control;
                    break;
            }
        }
    }

    public static bool HasAnyAsyncValidators(this FormComponentModel model) =>
        model.FlatFormControls().Any(control => control.Validators.Any());

    public static string GetTypeScriptTypeAnnotation(this FormControl control) => control switch
    {
        MultiSelectFormControl => "<unknown[] | null>",
        MultiCheckboxFormControl => "<unknown[] | null>",
        InputControlBase => "<string | null>",
        CheckboxFormControl => "<boolean>",
        _ => ""
    };

    public static string GetInitialValue(this FormControl control) => control switch
    {
        InputControlBase input when input.DefaultValue != null => $"'{input.DefaultValue}'",
        CheckboxFormControl { DefaultValue: not null } checkbox => checkbox.DefaultValue.Value.ToString().ToLower(),
        CheckboxFormControl => "false",
        _ => "null"
    };

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

    public static IHtmlContent ArrayItemFactoryMethod(this IHtmlHelper htmlHelper, ArrayFormControl arrayControl)
    {
        var group = (FormControlGroup)arrayControl.FormControlGroup;
        var propertyName = arrayControl.PropertyName;
        var methodName = Char.ToUpper(propertyName[0]) + propertyName.Substring(1);

        var childDeclarations = group.FormControls
            .Where(control => control is not ButtonFormControl)
            .Select(control => htmlHelper.FormControlDeclaration(control, "            ").ToString());

        return htmlHelper.Raw(
            $"    protected readonly create{methodName}Item = (): FormGroup => {{\r\n" +
            $"        return new FormGroup({{\r\n" +
            String.Concat(childDeclarations) +
            $"        }});\r\n" +
            $"    }};\r\n");
    }

    public static IHtmlContent AllArrayItemFactoryMethods(this IHtmlHelper htmlHelper, FormComponentModel model) =>
        htmlHelper.Raw(String.Concat(model.FlatFormControls().OfType<ArrayFormControl>()
            .Select(array => htmlHelper.ArrayItemFactoryMethod(array).ToString())));

    public static IHtmlContent ReEnableStaticReadonlyControls(this IHtmlHelper htmlHelper, FormComponentModel model)
    {
        var lines = model.FlatFormControls()
            .Where(control => control.Readonly && control is not ArrayFormControl)
            .Select(control => $"                this.form.get('{control.PropertyName}')?.disable({{ emitEvent: false }});");

        return htmlHelper.Raw(String.Join("\r\n", lines));
    }

    public static IHtmlContent SelectInputDeclarations(this IHtmlHelper htmlHelper, SelectControlBase select, bool enableI18N)
    {
        var lines = new List<string>();

        if (select.Options.HasDynamicValues)
        {
            lines.Add($"    protected readonly {select.PropertyName}Callback = input<Observable<unknown[]>>([]);");
            lines.Add($"    private readonly {select.PropertyName}Options = toSignal(this.{select.PropertyName}Callback(), {{ initialValue: [] }});");
        }
        else
        {
            var options = htmlHelper.JsArray(select.Options.FixedOptions,
                option => $"{{ {option.GetValueExpression()}, displayName: {htmlHelper.Localize(option.DisplayName, enableI18N)} }}");
            lines.Add($"    private readonly {select.PropertyName}Options = signal({options});");
        }

        if (select.Options.HasCustomValueAndDisplayKeys)
        {
            lines.Add($"    protected readonly {select.PropertyName}OptionsConfiguration = input<SelectConfiguration>({{ valueProperty: '{select.Options.OptionValueKey}', labelProperty: '{select.Options.OptionDisplayKey}', sortProperty: '{select.Options.OptionSortKey}' }});");
        }
        else
        {
            lines.Add($"    protected readonly {select.PropertyName}OptionsConfiguration = input<SelectConfiguration>({select.Options.DefaultOptionsAsString});");
        }

        return htmlHelper.Raw(String.Join("\r\n", lines) + "\r\n");
    }

    public static IHtmlContent AllSelectInputDeclarations(this IHtmlHelper htmlHelper, FormComponentModel model, bool enableI18N) =>
        htmlHelper.Raw(String.Concat(model.FormControlsOfType<SelectControlBase>()
            .Select(select => htmlHelper.SelectInputDeclarations(select, enableI18N).ToString())));

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
