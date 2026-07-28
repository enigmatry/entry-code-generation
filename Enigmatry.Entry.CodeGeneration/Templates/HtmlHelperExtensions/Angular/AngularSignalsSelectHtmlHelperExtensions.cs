using Enigmatry.Entry.CodeGeneration.Configuration;
using Enigmatry.Entry.CodeGeneration.Configuration.Form;
using Enigmatry.Entry.CodeGeneration.Configuration.Form.Controls;
using Enigmatry.Entry.CodeGeneration.Configuration.Form.Controls.Array;
using Enigmatry.Entry.CodeGeneration.Templates.HtmlHelperExtensions.TypeScript;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Enigmatry.Entry.CodeGeneration.Templates.HtmlHelperExtensions.Angular;

public static class AngularSignalsSelectHtmlHelperExtensions
{
    public static IHtmlContent SelectInputDeclarations(this IHtmlHelper htmlHelper, SelectControlBase select, bool enableI18N, string memberNamePrefix = "")
    {
        string MemberName(string suffix) => memberNamePrefix.Length == 0
            ? $"{select.PropertyName}{suffix}"
            : $"{memberNamePrefix}{AngularSignalsFormModelExtensions.Capitalize(select.PropertyName)}{suffix}";

        var lines = new List<string>();

        if (select.Options.HasDynamicValues)
        {
            lines.Add($"    protected readonly {MemberName("Callback")} = input<Observable<unknown[]> | null>(null);");
            lines.Add($"    private readonly {MemberName("RawOptions")} = toSignal(toObservable(this.{MemberName("Callback")}).pipe(switchMap(callback => callback ?? of([]))), {{ initialValue: [] }});");
        }
        else
        {
            var options = htmlHelper.JsArray(select.Options.FixedOptions,
                option => $"{{ {SelectOptionValueExpression(option)}, displayName: {htmlHelper.LocalizeEscaped(option.DisplayName, enableI18N)} }}");
            lines.Add($"    private readonly {MemberName("RawOptions")} = signal({options});");
        }

        if (select.Options.HasCustomValueAndDisplayKeys)
        {
            lines.Add($"    protected readonly {MemberName("OptionsConfiguration")} = input<SelectConfiguration>({{ valueProperty: '{select.Options.OptionValueKey}', labelProperty: '{select.Options.OptionDisplayKey}', sortProperty: '{select.Options.OptionSortKey}' }});");
        }
        else
        {
            lines.Add($"    protected readonly {MemberName("OptionsConfiguration")} = input<SelectConfiguration>({select.Options.DefaultOptionsAsString});");
        }

        lines.Add($"    protected readonly {MemberName("Options")} = computed(() => {{");
        lines.Add($"        const configuration = this.{MemberName("OptionsConfiguration")}();");
        lines.Add($"        const options = sortOptions(this.{MemberName("RawOptions")}() as Record<string, unknown>[], configuration.valueProperty ?? 'value', configuration.sortProperty ?? '', this.localeId) as Record<string, unknown>[];");
        lines.Add($"        return options.map(option => ({{ value: option[configuration.valueProperty ?? 'value'], displayName: option[configuration.labelProperty ?? 'displayName'] }}));");
        lines.Add($"    }});");

        if (select is AutocompleteFormControl)
        {
            var propertyNameCapitalized = AngularSignalsFormModelExtensions.Capitalize(select.PropertyName);
            lines.Add($"    protected readonly display{propertyNameCapitalized} = (value: unknown): string =>");
            lines.Add($"        (this.{MemberName("Options")}().find(option => option.value === value)?.displayName as string) ?? '';");
        }

        return htmlHelper.Raw(String.Join("\r\n", lines) + "\r\n");
    }

    // Signals-side mirror of SelectOption.GetValueExpression with string escaping; the shared
    // helper stays untouched because the deprecated Formly templates rely on its exact output.
    private static string SelectOptionValueExpression(SelectOption option)
    {
        var value = option.Value;
        if (value != null && value.IsNumeric())
        {
            return $"value: {(value is Enum ? (int)value : value)}";
        }

        if (value is bool boolValue)
        {
            return $"value: {boolValue.ToString().ToLowerInvariant()}";
        }

        return $"value: {(value == null ? "null" : $"'{value.ToString()!.EscapeTsSingleQuoted()}'")}";
    }

    public static IHtmlContent AllSelectInputDeclarations(this IHtmlHelper htmlHelper, FormComponentModel model, bool enableI18N) =>
        htmlHelper.Raw(String.Concat(model.SelectControlsWithMemberPrefixes()
            .Select(entry => htmlHelper.SelectInputDeclarations(entry.Select, enableI18N, entry.MemberNamePrefix).ToString())));

    internal static IEnumerable<(SelectControlBase Select, string MemberNamePrefix)> SelectControlsWithMemberPrefixes(this FormComponentModel model)
    {
        foreach (var select in model.FormControlsOfType<SelectControlBase>())
        {
            yield return (select, "");
        }

        foreach (var array in model.FlatFormControls().OfType<ArrayFormControl>())
        {
            var children = ((FormControlGroup)array.FormControlGroup).FormControls;
            foreach (var select in children.FlatFormControls().OfType<SelectControlBase>())
            {
                if (select is AutocompleteFormControl)
                {
                    throw new InvalidOperationException(
                        $"Autocomplete controls are not supported inside array items (per-row filtering state cannot be generated). " +
                        $"Property '{array.PropertyName}.{select.PropertyName}' on component '{model.ComponentInfo.Name}'.");
                }

                yield return (select, array.PropertyName);
            }
        }
    }

    public static IHtmlContent AllAutocompleteFilterDeclarations(this IHtmlHelper htmlHelper, FormComponentModel model)
    {
        var autocompleteControls = model.FormControlsOfType<AutocompleteFormControl>().ToList();
        if (autocompleteControls.Count == 0)
        {
            return htmlHelper.Raw("");
        }

        var declarations = autocompleteControls.Select(autocomplete =>
            $"    private readonly {autocomplete.PropertyName}FilterValue = toSignal(this.form.controls.{autocomplete.PropertyName}.valueChanges, {{ initialValue: null }});\r\n" +
            $"    protected readonly {autocomplete.PropertyName}FilteredOptions = computed(() => {{\r\n" +
            $"        const filterValue = this.{autocomplete.PropertyName}FilterValue();\r\n" +
            $"        const filterText = typeof filterValue === 'string' ? filterValue.toLowerCase() : '';\r\n" +
            $"        return this.{autocomplete.PropertyName}Options().filter(option =>\r\n" +
            $"            option.value === filterValue || String(option.displayName).toLowerCase().includes(filterText));\r\n" +
            $"    }});\r\n");

        return htmlHelper.Raw("\r\n" + String.Concat(declarations));
    }
}
